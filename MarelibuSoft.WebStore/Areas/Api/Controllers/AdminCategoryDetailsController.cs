using MarelibuSoft.WebStore.Areas.Api.ViewModels;
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
    [Route("api/AdminCategoryDetails")]
	//[Authorize(Roles = "Administrator, PowerUser")]
	public class AdminCategoryDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCategoryDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AdminCategoryDetails
        [HttpGet]
        public IEnumerable<CategoryDetail> GetCategoryDetails()
        {
            return _context.CategoryDetails;
        }

        // GET: api/AdminCategoryDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDetail = await _context.CategoryDetails.SingleOrDefaultAsync(m => m.ID == id);

            if (categoryDetail == null)
            {
                return NotFound();
            }

            return Ok(categoryDetail);
        }
		[HttpGet("Sub/{id}")]
		public async Task<IActionResult> GetCategoryDetailbySub([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var categoryDetail = await _context.CategoryDetails.Where(m => m.CategorySubID == id).ToArrayAsync();

			if (categoryDetail == null)
			{
				return NotFound();
			}

			List<OptionValue> vms = new List<OptionValue>();
			foreach (var item in categoryDetail)
			{
				var vm = new OptionValue { Key = item.ID, Value = item.Name };
				vms.Add(vm);
			}

			return Ok(vms);
		}

		// PUT: api/AdminCategoryDetails/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryDetail([FromRoute] int id, [FromBody] CategoryDetail categoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoryDetail.ID)
            {
                return BadRequest();
            }

            _context.Entry(categoryDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryDetailExists(id))
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

        // POST: api/AdminCategoryDetails
        [HttpPost]
        public async Task<IActionResult> PostCategoryDetail([FromBody] CategoryDetail categoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CategoryDetails.Add(categoryDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryDetail", new { id = categoryDetail.ID }, categoryDetail);
        }

        // DELETE: api/AdminCategoryDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDetail = await _context.CategoryDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            _context.CategoryDetails.Remove(categoryDetail);
            await _context.SaveChangesAsync();

            return Ok(categoryDetail);
        }

        private bool CategoryDetailExists(int id)
        {
            return _context.CategoryDetails.Any(e => e.ID == id);
        }
    }
}