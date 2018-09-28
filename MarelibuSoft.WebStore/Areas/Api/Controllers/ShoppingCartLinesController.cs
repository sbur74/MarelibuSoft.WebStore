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
		[Produces("application/json")]
		public async Task<IActionResult> GetShoppingCartLine([FromRoute] int id)
        {
			_logger.LogDebug($"api/ShoppingCartLines/GetShoppingCartLine id : {id}");
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
		[Produces("application/json")]
		public  IEnumerable<ShoppingCartLine> GetShoppingCartLinesByCart([FromRoute] Guid id)
		{
			return _context.ShoppingCartLines.Where(l => l.ShoppingCartID == id).ToList();
		}

		// PUT: api/ShoppingCartLines/5
		[HttpPut("{id}")]
		[ValidateAntiForgeryToken]
		[Produces("application/json")]
		public async Task<IActionResult> PutShoppingCartLine([FromRoute] int id, [FromBody] ShoppingCartLine shoppingCartLine)
        {
			_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine id:{id}");
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ModelState.IsValid });
            }

            if (id != shoppingCartLine.ID)
            {
                return BadRequest( new { message = "Die Id ist nicht bekannt" });
            }
			var cart = await _context.ShoppingCarts.SingleAsync(c => c.ID.Equals(shoppingCartLine.ShoppingCartID));
			var line = await _context.ShoppingCartLines.Where(s => s.ID.Equals(id)).SingleAsync();
			var product = await _context.Products.Where(p => p.ProductID.Equals(shoppingCartLine.ProductID)).SingleAsync();
			decimal lineQuantity = Math.Round(line.Quantity);

			_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity = {lineQuantity}, shoppingCartLine.Quantity = {shoppingCartLine.Quantity}");

			if(lineQuantity < shoppingCartLine.Quantity)
			{
				_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity < shoppingCartLine.Quantity");
				decimal diff = shoppingCartLine.Quantity - line.Quantity;
				_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity < shoppingCartLine.Quantity diff = {diff}, AvailableQuantity = {product.AvailableQuantity}");
				if (product.AvailableQuantity >= diff)
				{
					product.AvailableQuantity -= diff; 
				}
				else
				{
					return BadRequest("Nicht genung Ware verfügbar!");
				}
				_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity < shoppingCartLine.Quantity AvailableQuantity - diff = AvailableQuantity = {product.AvailableQuantity}");

			}
			if (lineQuantity > shoppingCartLine.Quantity)
			{
				_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity > shoppingCartLine.Quantity");
				decimal diff = line.Quantity - shoppingCartLine.Quantity;
				_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity > shoppingCartLine.Quantity diff = {diff}, AvailableQuantity = {product.AvailableQuantity}");
				product.AvailableQuantity += diff;
				_logger.LogDebug($"api/ShoppingCartLines/PutShoppingCartLine lineQuantity > shoppingCartLine.Quantity AvailableQuantity + diff = AvailableQuantity = {product.AvailableQuantity}");
			}
			
			
            try
            {
				line.Quantity = shoppingCartLine.Quantity;
				line.SellBasePrice = shoppingCartLine.SellBasePrice;

				cart.LastChange = DateTime.Now;
				_context.Update(product);
				_context.Update(line);
				_context.Update(cart);
				await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

				_logger.LogError(e, $"Api Error PutShoppingCartLine{id}", null);

                if (!ShoppingCartLineExists(id))
                {
                    return NotFound(new { message = "Zeile konnte nicht geändert werden!" });
                }
                else
                {
					return BadRequest(e);
                }

            }

            return Ok(new { id = shoppingCartLine.ID, productid = shoppingCartLine.ProductID });
        }

        // POST: api/ShoppingCartLines
        [HttpPost]
		[ValidateAntiForgeryToken]
		[Produces("application/json")]
		public async Task<ActionResult<ShoppingCartLine>> PostShoppingCartLine([FromBody] ShoppingCartLine shoppingCartLine)
        {
			int position = 1000;
			try
			{
				var lines = await _context.ShoppingCartLines.Where(l => l.ShoppingCartID == shoppingCartLine.ShoppingCartID).ToListAsync();

				var product = await _context.Products.Where(p => p.ProductID.Equals(shoppingCartLine.ProductID)).SingleAsync();

				var cart = await _context.ShoppingCarts.SingleAsync(c => c.ID.Equals(shoppingCartLine.ShoppingCartID));
				cart.LastChange = DateTime.Now;
				_context.ShoppingCarts.Update(cart);

				if (product.AvailableQuantity >= shoppingCartLine.Quantity)
				{
					product.AvailableQuantity -= shoppingCartLine.Quantity;
					_context.Products.Update(product);
				}
				else
				{
					return BadRequest(new { message = "Nicht genung Ware verfügbar!" });
				}



				if (lines != null && lines.Count() > 0)
				{
					position += lines.Count() + 1;
				}

				if (!ModelState.IsValid)
				{
					return BadRequest(new { message = ModelState });
				}

				bool isUpdate = false;

				foreach (ShoppingCartLine line in lines)
				{
					if (line.ProductID == shoppingCartLine.ProductID)
					{
						isUpdate = true;
						line.Quantity += shoppingCartLine.Quantity;
						_context.ShoppingCartLines.Update(line);
						break;
					}
				}

				if (!isUpdate)
				{
					if (shoppingCartLine.Quantity >= product.MinimumPurchaseQuantity)
					{
						shoppingCartLine.Position = position;
						_context.ShoppingCartLines.Add(shoppingCartLine);
					}
					else
					{
						return BadRequest(new { message = "Die Menge liegt unter Mindestabnahme!" });
					}
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Api Error Post ShoppingCartLine", null);
				return BadRequest(e);
			}

			//return CreatedAtAction(nameof(GetShoppingCartLine), new { id = shoppingCartLine.ID }, shoppingCartLine);
			//return CreatedAtAction("GetShoppingCartLine", shoppingCartLine);
			return Ok(new {id = shoppingCartLine.ID, productid = shoppingCartLine.ProductID});

		}

        // DELETE: api/ShoppingCartLines/5
        [HttpDelete("{id}")]
		[ValidateAntiForgeryToken]
		[Produces("application/json")]
		public async Task<IActionResult> DeleteShoppingCartLine([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ModelState });
            }

            var shoppingCartLine = await _context.ShoppingCartLines.SingleOrDefaultAsync(m => m.ID == id);
			
            if (shoppingCartLine == null)
            {
                return NotFound(new { message = "Zeile konnte nicht gelöscht werdern!" });
            }

			_logger.LogDebug($"api/ShoppingCartLines/DeleteShoppingCartLine id:{id}");

			var cart = await _context.ShoppingCarts.SingleAsync(c => c.ID.Equals(shoppingCartLine.ShoppingCartID));
			cart.LastChange = DateTime.Now;
			_context.ShoppingCarts.Update(cart);

			var product = await _context.Products.SingleAsync(p => p.ProductID.Equals(shoppingCartLine.ProductID));
			product.AvailableQuantity += shoppingCartLine.Quantity;

			_context.Products.Update(product);

			_context.ShoppingCartLines.Remove(shoppingCartLine);
            await _context.SaveChangesAsync();

			_logger.LogDebug($"api/ShoppingCartLines/DeleteShoppingCartLine return AvailableQuantity:{product.AvailableQuantity}");

            return Ok(product);
        }

        private bool ShoppingCartLineExists(int id)
        {
            return _context.ShoppingCartLines.Any(e => e.ID == id);
        }
    }
}