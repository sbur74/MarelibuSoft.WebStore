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
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Areas.Api.ViewModels;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AdminCategorySubs")]
	//[Authorize(Roles = "Administrator, PowerUser")]
	public class AdminCategorySubsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCategorySubsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AdminCategorySubs
        [HttpGet]
        public IEnumerable<CategorySub> GetCategorySubs()
        {
            return _context.CategorySubs;
        }

        // GET: api/AdminCategorySubs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategorySub([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categorySub = await _context.CategorySubs.SingleOrDefaultAsync(m => m.ID == id);

            if (categorySub == null)
            {
                return NotFound();
            }

            return Ok(categorySub);
        }
		[HttpGet("Category/{id}")]
		public async Task<IActionResult> GetCategorySubByCategory([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var categorySub = await _context.CategorySubs.Where(m => m.CategoryID == id).ToListAsync();

			if (categorySub == null)
			{
				return NotFound();
			}
			List<OptionValue> vms = new List<OptionValue>();

			foreach (var item in categorySub)
			{
				var vm = new OptionValue { Key = item.ID, Value = item.Name };
				vms.Add(vm);
			}

			return Ok(vms);
		}

		// PUT: api/AdminCategorySubs/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutCategorySub([FromRoute] int id, [FromBody] CategorySub categorySub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorySub.ID)
            {
                return BadRequest();
            }

            _context.Entry(categorySub).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorySubExists(id))
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

        // POST: api/AdminCategorySubs
        [HttpPost]
        public async Task<IActionResult> PostCategorySub([FromBody] CategorySub categorySub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CategorySubs.Add(categorySub);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategorySub", new { id = categorySub.ID }, categorySub);
        }

        // DELETE: api/AdminCategorySubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorySub([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categorySub = await _context.CategorySubs.SingleOrDefaultAsync(m => m.ID == id);
            if (categorySub == null)
            {
                return NotFound();
            }

            _context.CategorySubs.Remove(categorySub);
            await _context.SaveChangesAsync();

            return Ok(categorySub);
        }

        private bool CategorySubExists(int id)
        {
            return _context.CategorySubs.Any(e => e.ID == id);
        }
    }
}