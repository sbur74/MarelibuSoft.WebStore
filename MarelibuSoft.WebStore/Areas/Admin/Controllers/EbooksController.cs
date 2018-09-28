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
	public class EbooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EbooksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Admin/Ebooks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ebooks.ToListAsync());
        }

        // GET: Admin/Ebooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebooks
                .SingleOrDefaultAsync(m => m.EbookID == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        // GET: Admin/Ebooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Ebooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EbookID,Path,PathFileID,Name,Version,FkProductID")] Ebook ebook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ebook);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ebook);
        }

        // GET: Admin/Ebooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebooks.SingleOrDefaultAsync(m => m.EbookID == id);
            if (ebook == null)
            {
                return NotFound();
            }
            return View(ebook);
        }

        // POST: Admin/Ebooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int EbookID, [Bind("EbookID,Path,PathFileID,Name,Version,FkProductID")] Ebook ebook)
        {
            if (EbookID != ebook.EbookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EbookExists(ebook.EbookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(ebook);
        }

        // GET: Admin/Ebooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebooks
                .SingleOrDefaultAsync(m => m.EbookID == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        // POST: Admin/Ebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ebook = await _context.Ebooks.SingleOrDefaultAsync(m => m.EbookID == id);
            _context.Ebooks.Remove(ebook);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EbookExists(int id)
        {
            return _context.Ebooks.Any(e => e.EbookID == id);
        }
    }
}
