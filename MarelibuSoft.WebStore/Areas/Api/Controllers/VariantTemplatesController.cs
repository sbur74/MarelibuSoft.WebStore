using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateAntiForgeryToken]
    public class VariantTemplatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VariantTemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VariantTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VariantTemplate>>> GetVariantTemplates()
        {
            return await _context.VariantTemplates.Include(ot => ot.VariantOptionTemplates).ToListAsync();
        }

        // GET: api/VariantTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VariantTemplate>> GetVariantTemplate(int id)
        {
            var variantTemplate = await _context.VariantTemplates.FindAsync(id);

            if (variantTemplate == null)
            {
                return NotFound();
            }

            return variantTemplate;
        }

        // PUT: api/VariantTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVariantTemplate(int id, VariantTemplate variantTemplate)
        {
            if (id != variantTemplate.ID)
            {
                return BadRequest();
            }

            _context.Entry(variantTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantTemplateExists(id))
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

        // POST: api/VariantTemplates
        [HttpPost]
        public async Task<ActionResult<VariantTemplate>> PostVariantTemplate(VariantTemplate variantTemplate)
        {
            _context.VariantTemplates.Add(variantTemplate);
            await _context.SaveChangesAsync();

            return Ok(new { id = variantTemplate.ID });
        }

        // DELETE: api/VariantTemplates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VariantTemplate>> DeleteVariantTemplate(int id)
        {
            var variantTemplate = await _context.VariantTemplates.FindAsync(id);
            if (variantTemplate == null)
            {
                return NotFound();
            }

            _context.VariantTemplates.Remove(variantTemplate);
            await _context.SaveChangesAsync();

            return variantTemplate;
        }

        private bool VariantTemplateExists(int id)
        {
            return _context.VariantTemplates.Any(e => e.ID == id);
        }
    }
}
