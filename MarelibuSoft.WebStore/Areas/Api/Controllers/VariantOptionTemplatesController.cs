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
    [Route("api/[controller]")]
    [ApiController]
    public class VariantOptionTemplatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VariantOptionTemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VariantOptionTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VariantOptionTemplate>>> GetVariantOptionTemplates()
        {
            return await _context.VariantOptionTemplates.ToListAsync();
        }

        // GET: api/VariantOptionTemplates/5 
        [HttpGet("{id}")]
        public async Task<ActionResult<VariantOptionTemplate>> GetVariantOptionTemplate(int id)
        {
            var variantOptionTemplate = await _context.VariantOptionTemplates.FindAsync(id);

            if (variantOptionTemplate == null)
            {
                return NotFound();
            }

            return variantOptionTemplate;
        }
        // GET: api/VariantOptionTemplates/variant/5 
        [HttpGet("variant/{id}")]
        public async Task<ActionResult<IEnumerable<VariantOptionTemplate>>> GetVariantOptionTemplatesByTemplateId(int id)
        {
            var variantOptionTemplates = await _context.VariantOptionTemplates.Where(o => o.VariantTemplateId == id).ToListAsync();

            if (variantOptionTemplates == null)
            {
                return NotFound();
            }

            return variantOptionTemplates;
        }

        // PUT: api/VariantOptionTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVariantOptionTemplate(int id, VariantOptionTemplate variantOptionTemplate)
        {
            if (id != variantOptionTemplate.ID)
            {
                return BadRequest();
            }

            _context.Entry(variantOptionTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantOptionTemplateExists(id))
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

        // POST: api/VariantOptionTemplates
        [HttpPost]
        public async Task<ActionResult<VariantOptionTemplate>> PostVariantOptionTemplate(VariantOptionTemplate variantOptionTemplate)
        {
            _context.VariantOptionTemplates.Add(variantOptionTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVariantOptionTemplate", new { id = variantOptionTemplate.ID }, variantOptionTemplate);
        }

        // DELETE: api/VariantOptionTemplates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VariantOptionTemplate>> DeleteVariantOptionTemplate(int id)
        {
            var variantOptionTemplate = await _context.VariantOptionTemplates.FindAsync(id);
            if (variantOptionTemplate == null)
            {
                return NotFound();
            }

            _context.VariantOptionTemplates.Remove(variantOptionTemplate);
            await _context.SaveChangesAsync();

            return variantOptionTemplate;
        }

        private bool VariantOptionTemplateExists(int id)
        {
            return _context.VariantOptionTemplates.Any(e => e.ID == id);
        }
    }
}
