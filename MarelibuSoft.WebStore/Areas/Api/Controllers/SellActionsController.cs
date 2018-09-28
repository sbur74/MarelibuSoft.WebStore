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
    [Route("api/SellActions")]
    public class SellActionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellActionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SellActions
        [HttpGet]
        public IEnumerable<SellAction> GetSellActions()
        {
            return _context.SellActions;
        }

        // GET: api/SellActions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSellAction([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sellAction = await _context.SellActions.SingleOrDefaultAsync(m => m.SellActionID == id);

            if (sellAction == null)
            {
                return NotFound();
            }

            return Ok(sellAction);
        }

        // PUT: api/SellActions/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSellAction([FromRoute] int id, [FromBody] SellAction sellAction)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != sellAction.SellActionID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(sellAction).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SellActionExists(id))
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

        // POST: api/SellActions
        //[HttpPost]
        //public async Task<IActionResult> PostSellAction([FromBody] SellAction sellAction)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.SellActions.Add(sellAction);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetSellAction", new { id = sellAction.SellActionID }, sellAction);
        //}

        // DELETE: api/SellActions/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSellAction([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var sellAction = await _context.SellActions.SingleOrDefaultAsync(m => m.SellActionID == id);
        //    if (sellAction == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SellActions.Remove(sellAction);
        //    await _context.SaveChangesAsync();

        //    return Ok(sellAction);
        //}

        private bool SellActionExists(int id)
        {
            return _context.SellActions.Any(e => e.SellActionID == id);
        }
    }
}