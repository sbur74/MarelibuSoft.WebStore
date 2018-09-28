using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ShoppingCarts")]
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCarts
        [HttpGet]
		[ValidateAntiForgeryToken]
		public IEnumerable<ShoppingCart> GetShoppingCarts()
        {
            return _context.ShoppingCarts;
        }

        // GET: api/ShoppingCarts/5
        [HttpGet("{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GetShoppingCart([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.ID == id);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }

        // PUT: api/ShoppingCarts/5
        [HttpPut("{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PutShoppingCart([FromRoute] Guid id, [FromBody] ShoppingCart shoppingCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoppingCart.ID)
            {
                return BadRequest();
            }

            _context.Entry(shoppingCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ShoppingCarts
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PostShoppingCart([FromBody] ShoppingCart shoppingCart)
        {
			shoppingCart.Number = "MD" + DateTime.Now.Ticks;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCart", new { id = shoppingCart.ID }, shoppingCart);
        }

        // DELETE: api/ShoppingCarts/5
        [HttpDelete("{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteShoppingCart([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();

            return Ok(shoppingCart);
        }

        private bool ShoppingCartExists(Guid id)
        {
            return _context.ShoppingCarts.Any(e => e.ID == id);
        }
    }
}