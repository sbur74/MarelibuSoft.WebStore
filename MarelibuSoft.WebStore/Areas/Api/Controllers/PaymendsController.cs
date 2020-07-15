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
    [Route("api/Paymends")]
    public class PaymendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Paymends
        [HttpGet]
        public IEnumerable<Paymend> GetPaymends()
        {
            return _context.Paymends;
        }

        // GET: api/Paymends/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymend([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymend = await _context.Paymends.SingleOrDefaultAsync(m => m.ID == id);

            if (paymend == null)
            {
                return NotFound();
            }

            return Ok(paymend);
        }

        // PUT: api/Paymends/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPaymend([FromRoute] int id, [FromBody] Paymend paymend)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != paymend.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(paymend).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PaymendExists(id))
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

        // POST: api/Paymends
        //[HttpPost]
        //public async Task<IActionResult> PostPaymend([FromBody] Paymend paymend)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Paymends.Add(paymend);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPaymend", new { id = paymend.ID }, paymend);
        //}

        // DELETE: api/Paymends/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePaymend([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var paymend = await _context.Paymends.SingleOrDefaultAsync(m => m.ID == id);
        //    if (paymend == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Paymends.Remove(paymend);
        //    await _context.SaveChangesAsync();

        //    return Ok(paymend);
        //}

        private bool PaymendExists(int id)
        {
            return _context.Paymends.Any(e => e.ID == id);
        }
    }
}