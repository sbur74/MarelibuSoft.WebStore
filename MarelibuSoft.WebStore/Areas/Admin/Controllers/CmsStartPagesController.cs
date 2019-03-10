using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CmsStartPagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CmsStartPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CmsStartPages
        public async Task<IActionResult> Index()
        {
            return View(await _context.CmsStartPages.ToListAsync());
        }

        // GET: Admin/CmsStartPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmsStartPage = await _context.CmsStartPages
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cmsStartPage == null)
            {
                return NotFound();
            }

            return View(cmsStartPage);
        }

        // GET: Admin/CmsStartPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CmsStartPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,HeadContent,LeftContent,RightContent,SeoKeywords,SeoDescription")] CmsStartPage cmsStartPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cmsStartPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cmsStartPage);
        }

        // GET: Admin/CmsStartPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmsStartPage = await _context.CmsStartPages.FindAsync(id);
            if (cmsStartPage == null)
            {
                return NotFound();
            }
            return View(cmsStartPage);
        }

        // POST: Admin/CmsStartPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,HeadContent,LeftContent,RightContent,SeoKeywords,SeoDescription")] CmsStartPage cmsStartPage)
        {
            if (id != cmsStartPage.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cmsStartPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CmsStartPageExists(cmsStartPage.ID))
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
            return View(cmsStartPage);
        }

        // GET: Admin/CmsStartPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cmsStartPage = await _context.CmsStartPages
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cmsStartPage == null)
            {
                return NotFound();
            }

            return View(cmsStartPage);
        }

        // POST: Admin/CmsStartPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cmsStartPage = await _context.CmsStartPages.FindAsync(id);
            _context.CmsStartPages.Remove(cmsStartPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CmsStartPageExists(int id)
        {
            return _context.CmsStartPages.Any(e => e.ID == id);
        }
    }
}
