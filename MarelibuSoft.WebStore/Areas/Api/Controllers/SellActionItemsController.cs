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
    [Route("api/SellActionItems")]
    public class SellActionItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellActionItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SellActionItems
        [HttpGet]
        public IEnumerable<SellActionItem> GetSellActionItems()
        {
            return _context.SellActionItems;
        }

        // GET: api/SellActionItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSellActionItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sellActionItem = await _context.SellActionItems.SingleOrDefaultAsync(m => m.SellActionItemID == id);

            if (sellActionItem == null)
            {
                return NotFound();
            }

            return Ok(sellActionItem);
        }

        // PUT: api/SellActionItems/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSellActionItem([FromRoute] int id, [FromBody] SellActionItem sellActionItem)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != sellActionItem.SellActionItemID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(sellActionItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SellActionItemExists(id))
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

        // POST: api/SellActionItems
        //[HttpPost]
        //public async Task<IActionResult> PostSellActionItem([FromBody] SellActionItem sellActionItem)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.SellActionItems.Add(sellActionItem);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetSellActionItem", new { id = sellActionItem.SellActionItemID }, sellActionItem);
        //}

        // DELETE: api/SellActionItems/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSellActionItem([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var sellActionItem = await _context.SellActionItems.SingleOrDefaultAsync(m => m.SellActionItemID == id);
        //    if (sellActionItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SellActionItems.Remove(sellActionItem);
        //    await _context.SaveChangesAsync();

        //    return Ok(sellActionItem);
        //}

        private bool SellActionItemExists(int id)
        {
            return _context.SellActionItems.Any(e => e.SellActionItemID == id);
        }
    }
}