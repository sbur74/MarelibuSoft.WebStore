using MarelibuSoft.WebStore.Areas.Api.ViewModels;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Produces("application/json")]
	[Route("api/Products")]
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ProductsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Products
		[HttpGet]
		[ValidateAntiForgeryToken]
		public IEnumerable<Product> GetProducts()
		{
			return _context.Products;
		}

		// GET: api/Products/5
		[HttpGet("{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GetProduct([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);

			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutActiveProduct([FromRoute] int id, [FromBody] ActiveViewModel myactive)
		{
			if (id == 0)
			{
				return BadRequest("Kein Produkt gefunden");
			}
			var product = await _context.Products.SingleAsync(p => p.ProductID == id);

			if (product == null)
			{
				return NotFound("Kein Produkt gefunden");
			}

			product.IsActive = myactive.IsActive;
			_context.Products.Update(product);
			await _context.SaveChangesAsync(product.IsActive);


			return Ok(new { statusCode = 200 });
		}

		// PUT: api/Products/5
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        return BadRequest(ModelState);
		//    }

		//    if (id != product.ProductID)
		//    {
		//        return BadRequest();
		//    }

		//    _context.Entry(product).State = EntityState.Modified;

		//    try
		//    {
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!ProductExists(id))
		//        {
		//            return NotFound();
		//        }
		//        else
		//        {
		//            throw;
		//        }
		//    }

		//    return NoContent();
		//}

		// POST: api/Products
		//[HttpPost]
		//public async Task<IActionResult> PostProduct([FromBody] Product product)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        return BadRequest(ModelState);
		//    }

		//    _context.Products.Add(product);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
		//}

		// DELETE: api/Products/5
		//[HttpDelete("{id}")]
		//public async Task<IActionResult> DeleteProduct([FromRoute] int id)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        return BadRequest(ModelState);
		//    }

		//    var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
		//    if (product == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.Products.Remove(product);
		//    await _context.SaveChangesAsync();

		//    return Ok(product);
		//}

		private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}