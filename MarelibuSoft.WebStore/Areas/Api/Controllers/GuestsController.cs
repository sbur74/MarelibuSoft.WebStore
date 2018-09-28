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
    [Route("api/Guests")]
    public class GuestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Guests
        [HttpGet]
        public IEnumerable<Guest> GetGuest()
        {
            return _context.Guests;
        }

        // GET: api/Guests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guest = await _context.Guests.SingleOrDefaultAsync(m => m.GuestID == id);

            if (guest == null)
            {
                return NotFound();
            }

            return Ok(guest);
        }

        // PUT: api/Guests/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGuest([FromRoute] int id, [FromBody] Guest guest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != guest.GuestID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(guest).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GuestExists(id))
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

        // POST: api/Guests
        //[HttpPost]
        //public async Task<IActionResult> PostGuest([FromBody] Guest guest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Guest.Add(guest);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGuest", new { id = guest.GuestID }, guest);
        //}

        // DELETE: api/Guests/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGuest([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var guest = await _context.Guest.SingleOrDefaultAsync(m => m.GuestID == id);
        //    if (guest == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Guest.Remove(guest);
        //    await _context.SaveChangesAsync();

        //    return Ok(guest);
        //}

        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.GuestID == id);
        }
    }
}