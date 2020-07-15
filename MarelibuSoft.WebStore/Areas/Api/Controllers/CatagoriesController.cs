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
    [Route("api/Catagories")]
    public class CatagoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatagoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Catagories
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories;
        }

        // GET: api/Catagories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var catagory = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);

            if (catagory == null)
            {
                return NotFound();
            }

            return Ok(catagory);
        }

        // PUT: api/Catagories/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCatagory([FromRoute] int id, [FromBody] Catagory catagory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != catagory.CatagoryID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(catagory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CatagoryExists(id))
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

        // POST: api/Catagories
        //[HttpPost]
        //public async Task<IActionResult> PostCatagory([FromBody] Catagory catagory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Catagories.Add(catagory);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCategory", new { id = catagory.CatagoryID }, catagory);
        //}

        // DELETE: api/Catagories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCatagory([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var catagory = await _context.Catagories.SingleOrDefaultAsync(m => m.CatagoryID == id);
        //    if (catagory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Catagories.Remove(catagory);
        //    await _context.SaveChangesAsync();

        //    return Ok(catagory);
        //}

        private bool CatagoryExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}