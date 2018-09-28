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
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AdminCategoryAssignments")]
	//[Authorize(Roles = "Administrator, PowerUser")]
	public class AdminCategoryAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILoggerFactory factory;
		private readonly ILogger logger;

		public AdminCategoryAssignmentsController(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
			factory = loggerFactory;
			logger = factory.CreateLogger<AdminCategoryAssignmentsController>();
        }

        // GET: api/AdminCategoryAssignments
        [HttpGet]
        public IEnumerable<CategoryAssignment> GetCategoryAssignments()
        {
            return _context.CategoryAssignments;
        }

        // GET: api/AdminCategoryAssignments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryAssignment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryAssignment = await _context.CategoryAssignments.SingleOrDefaultAsync(m => m.ID == id);

            if (categoryAssignment == null)
            {
                return NotFound();
            }

            return Ok(categoryAssignment);
        }

        // PUT: api/AdminCategoryAssignments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryAssignment([FromRoute] int id, [FromBody] CategoryAssignment categoryAssignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoryAssignment.ID)
            {
                return BadRequest();
            }

            _context.Entry(categoryAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryAssignmentExists(id))
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

        // POST: api/AdminCategoryAssignments
        [HttpPost]
        public async Task<IActionResult> PostCategoryAssignment([FromBody] CategoryAssignment categoryAssignment)
        {
            if (!ModelState.IsValid)
            {
				logger.LogError("POST: api/AdminCategoryAssignments -> invalid model");
                return BadRequest(ModelState);
            }

			try
			{

				_context.CategoryAssignments.Add(categoryAssignment);
				await _context.SaveChangesAsync();

				return CreatedAtAction("GetCategoryAssignment", new { id = categoryAssignment.ID }, categoryAssignment);
			}
			catch (Exception e)
			{
				logger.LogError(e, "POST: api/AdminCategoryAssignments -> Fehler beim anlegen");
				return BadRequest(e.Message);
			}
        }

        // DELETE: api/AdminCategoryAssignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAssignment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryAssignment = await _context.CategoryAssignments.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryAssignment == null)
            {
                return NotFound();
            }

            _context.CategoryAssignments.Remove(categoryAssignment);
            await _context.SaveChangesAsync();

            return Ok(categoryAssignment);
        }

        private bool CategoryAssignmentExists(int id)
        {
            return _context.CategoryAssignments.Any(e => e.ID == id);
        }
    }
}