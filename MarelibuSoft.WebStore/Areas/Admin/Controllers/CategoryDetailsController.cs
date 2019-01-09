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
	public class CategoryDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CategoryDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CategoryDetails.Include(c => c.Sub);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/CategoryDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetail = await _context.CategoryDetails
                .Include(c => c.Sub)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            return View(categoryDetail);
        }

        // GET: Admin/CategoryDetails/Create
        public IActionResult Create(int? id)
        {
            ViewData["CategorySubID"] = new SelectList(_context.CategorySubs, "ID", "Name");
			CategoryDetail detail = new CategoryDetail();
			if (id != null)
			{
				CategorySub sub = _context.CategorySubs.Where(s => s.ID == id).Single();
				if (sub != null)
				{
					detail.CategorySubID = sub.ID;
					ViewData["SenderID"] = sub.ID;
				}
			}
            return View(detail);
        }

        // POST: Admin/CategoryDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,CategorySubID,SeoDescription,HtmlDescription,SeoKeywords")] CategoryDetail categoryDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorySubID"] = new SelectList(_context.CategorySubs, "ID", "Name", categoryDetail.CategorySubID);
            return View(categoryDetail);
        }

        // GET: Admin/CategoryDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetail = await _context.CategoryDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }
            ViewData["CategorySubID"] = new SelectList(_context.CategorySubs, "ID", "Name", categoryDetail.CategorySubID);
            return View(categoryDetail);
        }

        // POST: Admin/CategoryDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CategorySubID,SeoDescription,HtmlDescription,SeoKeywords")] CategoryDetail categoryDetail)
        {
            if (id != categoryDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryDetailExists(categoryDetail.ID))
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
            ViewData["CategorySubID"] = new SelectList(_context.CategorySubs, "ID", "Name", categoryDetail.CategorySubID);
            return View(categoryDetail);
        }

        // GET: Admin/CategoryDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDetail = await _context.CategoryDetails
                .Include(c => c.Sub)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            return View(categoryDetail);
        }

        // POST: Admin/CategoryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryDetail = await _context.CategoryDetails.SingleOrDefaultAsync(m => m.ID == id);
            _context.CategoryDetails.Remove(categoryDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryDetailExists(int id)
        {
            return _context.CategoryDetails.Any(e => e.ID == id);
        }
    }
}
