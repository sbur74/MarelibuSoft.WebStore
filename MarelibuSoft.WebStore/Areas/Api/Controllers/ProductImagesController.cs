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
    [Route("api/ProductImages")]
    public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductImages
        [HttpGet]
        public IEnumerable<ProductImage> GetProductImages()
        {
            return _context.ProductImages;
        }

        // GET: api/ProductImages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productImage = await _context.ProductImages.SingleOrDefaultAsync(m => m.ProductImageID == id);

            if (productImage == null)
            {
                return NotFound();
            }

            return Ok(productImage);
        }

        // PUT: api/ProductImages/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProductImage([FromRoute] int id, [FromBody] ProductImage productImage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != productImage.ProductImageID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(productImage).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductImageExists(id))
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

        // POST: api/ProductImages
        //[HttpPost]
        //public async Task<IActionResult> PostProductImage([FromBody] ProductImage productImage)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.ProductImages.Add(productImage);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProductImage", new { id = productImage.ProductImageID }, productImage);
        //}

        // DELETE: api/ProductImages/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProductImage([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var productImage = await _context.ProductImages.SingleOrDefaultAsync(m => m.ProductImageID == id);
        //    if (productImage == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ProductImages.Remove(productImage);
        //    await _context.SaveChangesAsync();

        //    return Ok(productImage);
        //}

        private bool ProductImageExists(int id)
        {
            return _context.ProductImages.Any(e => e.ProductImageID == id);
        }
    }
}