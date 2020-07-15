using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator, PowerUser")]
    [ValidateAntiForgeryToken]
    public class ProductVariantOptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductVariantOptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariantOption>>> GetProductVariantOptions()
        {
            return await _context.ProductVariantOptions.ToListAsync();
        }

        [HttpGet("variant/{id}")]
        public async Task<ActionResult<IEnumerable<ProductVariantOption>>> GetProductVariantOptions([FromRoute]int id)
        {
            return await _context.ProductVariantOptions.Where(o => o.ProductVariantID == id).ToListAsync();
        }

        // GET: api/ProductVariantOptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariantOption>> GetProductVariantOption(int id)
        {
            var productVariantOption = await _context.ProductVariantOptions.FindAsync(id);

            if (productVariantOption == null)
            {
                return NotFound();
            }

            return productVariantOption;
        }

        // PUT: api/ProductVariantOptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductVariantOption(int id, ProductVariantOption productVariantOption)
        {
            if (id != productVariantOption.ID)
            {
                return BadRequest();
            }

            _context.Entry(productVariantOption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductVariantOptionExists(id))
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

        // POST: api/ProductVariantOptions
        [HttpPost]
        public async Task<ActionResult<ProductVariantOption>> PostProductVariantOption(ProductVariantOption productVariantOption)
        {
            _context.ProductVariantOptions.Add(productVariantOption);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductVariantOption", new { id = productVariantOption.ID }, productVariantOption);
        }

        // DELETE: api/ProductVariantOptions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductVariantOption>> DeleteProductVariantOption(int id)
        {
            var productVariantOption = await _context.ProductVariantOptions.FindAsync(id);
            if (productVariantOption == null)
            {
                return NotFound();
            }

            _context.ProductVariantOptions.Remove(productVariantOption);
            await _context.SaveChangesAsync();

            return productVariantOption;
        }

        private bool ProductVariantOptionExists(int id)
        {
            return _context.ProductVariantOptions.Any(e => e.ID == id);
        }
    }
}
