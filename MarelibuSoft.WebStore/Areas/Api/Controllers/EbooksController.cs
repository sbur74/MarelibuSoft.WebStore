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
    [Route("api/Ebooks")]
    public class EbooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EbooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ebooks
        [HttpGet]
        public IEnumerable<Ebook> GetEbooks()
        {
            return _context.Ebooks;
        }

        // GET: api/Ebooks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEbook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ebook = await _context.Ebooks.SingleOrDefaultAsync(m => m.EbookID == id);

            if (ebook == null)
            {
                return NotFound();
            }

            return Ok(ebook);
        }

        // PUT: api/Ebooks/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEbook([FromRoute] int id, [FromBody] Ebook ebook)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != ebook.EbookID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(ebook).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EbookExists(id))
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

        // POST: api/Ebooks
        //[HttpPost]
        //public async Task<IActionResult> PostEbook([FromBody] Ebook ebook)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Ebooks.Add(ebook);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEbook", new { id = ebook.EbookID }, ebook);
        //}

        // DELETE: api/Ebooks/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEbook([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var ebook = await _context.Ebooks.SingleOrDefaultAsync(m => m.EbookID == id);
        //    if (ebook == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Ebooks.Remove(ebook);
        //    await _context.SaveChangesAsync();

        //    return Ok(ebook);
        //}

        private bool EbookExists(int id)
        {
            return _context.Ebooks.Any(e => e.EbookID == id);
        }
    }
}