using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Authorization;
using MarelibuSoft.WebStore.Areas.Api.ViewModels;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "Administrator, PowerUser")]
    [ValidateAntiForgeryToken]
    public class ProductVariantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductVariants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetProductVariants()
        {
            return await _context.ProductVariants.Include(o => o.Options).ToListAsync();
        }

        // GET: api/ProductVariants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariant>> GetProductVariant(int id)
        {
            var productVariant = await _context.ProductVariants.Include(o=>o.Options).SingleAsync(v => v.ID==id);

            if (productVariant == null)
            {
                return NotFound();
            }

            return productVariant;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductVariant(int id, ProductVariant productVariant)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Entry(productVariant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductVariantExists(id))
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

        // PUT: api/ProductVariants/5
        [HttpPut("isNecessary/{id}")]
        public async Task<IActionResult> PutProductVariantIsNecessary(int id, ActiveViewModel active)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var productVariant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.ID == id);
            if(productVariant == null)
            {
                return BadRequest();
            }
            productVariant.IsAbsolutelyNecessary = active.IsActive;
            _context.Entry(productVariant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductVariantExists(id))
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

        // POST: api/ProductVariants
        [HttpPost]
        public async Task<ActionResult<ProductVariant>> PostProductVariant(ProductVariant productVariant)
        {
            _context.ProductVariants.Add(productVariant);
            await _context.SaveChangesAsync();

            return Ok(new { id = productVariant.ID });
        }

        // DELETE: api/ProductVariants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductVariant>> DeleteProductVariant(int id)
        {
            var productVariant = await _context.ProductVariants.FindAsync(id);
            if (productVariant == null)
            {
                return NotFound();
            }

            _context.ProductVariants.Remove(productVariant);
            await _context.SaveChangesAsync();

            return productVariant;
        }

        private bool ProductVariantExists(int id)
        {
            return _context.ProductVariants.Any(e => e.ID == id);
        }
    }
}
