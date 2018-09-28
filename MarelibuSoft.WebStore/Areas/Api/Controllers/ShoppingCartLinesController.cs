using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ShoppingCartLines")]
    public class ShoppingCartLinesController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;

        public ShoppingCartLinesController(ApplicationDbContext context, ILogger<ShoppingCartLinesController>logger)
        {
            _context = context;
			_logger = logger;
        }

        // GET: api/ShoppingCartLines
        [HttpGet]
        public IEnumerable<ShoppingCartLine> GetShoppingCartLines()
        {
            return _context.ShoppingCartLines;
        }

        // GET: api/ShoppingCartLines/5
        [HttpGet("{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GetShoppingCartLine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCartLine = await _context.ShoppingCartLines.SingleOrDefaultAsync(m => m.ID == id);

            if (shoppingCartLine == null)
            {
                return NotFound();
            }

            return Ok(shoppingCartLine);
        }

		[HttpGet("cart/{id}")]
		[ValidateAntiForgeryToken]
		public  IEnumerable<ShoppingCartLine> GetShoppingCartLinesByCart([FromRoute] Guid id)
		{
			return _context.ShoppingCartLines.Where(l => l.ShoppingCartID == id).ToList();
		}

		// PUT: api/ShoppingCartLines/5
		[HttpPut("{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PutShoppingCartLine([FromRoute] int id, [FromBody] ShoppingCartLine shoppingCartLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoppingCartLine.ID)
            {
                return BadRequest();
            }

			var line = await _context.ShoppingCartLines.Where(s => s.ID.Equals(id)).SingleAsync();
			var product = await _context.Products.Where(p => p.ProductID.Equals(shoppingCartLine.ProductID)).SingleAsync();

			if(line.Quantity < shoppingCartLine.Quantity)
			{
				decimal diff = shoppingCartLine.Quantity - line.Quantity;
				product.AvailableQuantity -= diff;
			}
			if (line.Quantity > shoppingCartLine.Quantity)
			{
				decimal diff = line.Quantity - shoppingCartLine.Quantity;
				product.AvailableQuantity += diff;
			}

			_context.Entry(product).State = EntityState.Modified;
            _context.Entry(shoppingCartLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

				_logger.LogError(e, $"Api Error PutShoppingCartLine{id}", null);

                if (!ShoppingCartLineExists(id))
                {
                    return NotFound();
                }
                else
                {
					return BadRequest(e);
                }

            }

            return NoContent();
        }

        // POST: api/ShoppingCartLines
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PostShoppingCartLine([FromBody] ShoppingCartLine shoppingCartLine)
        {
			int position = 1000;
			try
			{
				var lines = await _context.ShoppingCartLines.Where(l => l.ShoppingCartID == shoppingCartLine.ShoppingCartID).ToListAsync();

				var product = await _context.Products.Where(p => p.ProductID.Equals(shoppingCartLine.ProductID)).SingleAsync();

				product.AvailableQuantity -= shoppingCartLine.Quantity;

				if (lines != null && lines.Count() > 0)
				{
					position += lines.Count() + 1;
				}

				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				bool isUpdate = false;

				foreach (ShoppingCartLine line in lines)
				{
					if (line.ProductID == shoppingCartLine.ProductID)
					{
						isUpdate = true;
						line.Quantity += shoppingCartLine.Quantity;
						_context.Entry(line).State = EntityState.Modified;
						break;
					}
				}

				if (!isUpdate)
				{
					shoppingCartLine.Position = position;
					_context.ShoppingCartLines.Add(shoppingCartLine);
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Api Error Post ShoppingCartLine", null);
				return BadRequest(e);
			}

			return CreatedAtAction("GetShoppingCartLine", new { id = shoppingCartLine.ID }, shoppingCartLine);
        }

        // DELETE: api/ShoppingCartLines/5
        [HttpDelete("{id}")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteShoppingCartLine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCartLine = await _context.ShoppingCartLines.SingleOrDefaultAsync(m => m.ID == id);
			
            if (shoppingCartLine == null)
            {
                return NotFound();
            }

			var product = await _context.Products.SingleAsync(p => p.ProductID.Equals(shoppingCartLine.ProductID));
			product.AvailableQuantity += shoppingCartLine.Quantity;

			_context.Entry(product).State = EntityState.Modified;

			_context.ShoppingCartLines.Remove(shoppingCartLine);
            await _context.SaveChangesAsync();

            return Ok(shoppingCartLine);
        }

        private bool ShoppingCartLineExists(int id)
        {
            return _context.ShoppingCartLines.Any(e => e.ID == id);
        }
    }
}