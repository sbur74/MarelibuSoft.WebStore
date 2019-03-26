using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class CategorySubsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategorySubsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CategorySubs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CategorySubs.Include(c => c.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/CategorySubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorySub = await _context.CategorySubs
                .Include(c => c.Category).Include(d => d.Details)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categorySub == null)
            {
                return NotFound();
            }

            return View(categorySub);
        }

        // GET: Admin/CategorySubs/Create
        public IActionResult Create(int? id)
        {
			CategorySub sub = new CategorySub();
			Category category = null;
			if (id != null)
			{
				 category = _context.Categories.Single(c => c.ID == id);
				if (sub != null)
				{
					sub.CategoryID = category.ID;
					ViewData["SenderID"] = category.ID; 
				}
			}
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderByDescending(c => c.ID), "ID", "Name");
			
            return View(sub);
        }

        // POST: Admin/CategorySubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,CategoryID,SeoDescription,HtmlDescription,SeoKeywords")] CategorySub categorySub)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorySub);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", categorySub.CategoryID);
            return View(categorySub);
        }

        // GET: Admin/CategorySubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorySub = await _context.CategorySubs.SingleOrDefaultAsync(m => m.ID == id);
            if (categorySub == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderByDescending(c => c.ID), "ID", "Name", categorySub.CategoryID);
            return View(categorySub);
        }

        // POST: Admin/CategorySubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CategoryID,SeoDescription,HtmlDescription,SeoKeywords")] CategorySub categorySub)
        {
            if (id != categorySub.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorySub);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorySubExists(categorySub.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", categorySub.CategoryID);
            return View(categorySub);
        }

        // GET: Admin/CategorySubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorySub = await _context.CategorySubs
                .Include(c => c.Category)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categorySub == null)
            {
                return NotFound();
            }

            return View(categorySub);
        }

        // POST: Admin/CategorySubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorySub = await _context.CategorySubs.SingleOrDefaultAsync(m => m.ID == id);
            _context.CategorySubs.Remove(categorySub);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategorySubExists(int id)
        {
            return _context.CategorySubs.Any(e => e.ID == id);
        }
    }
}
