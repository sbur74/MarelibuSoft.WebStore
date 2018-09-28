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
    public class OrderCompletionTextsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderCompletionTextsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderCompletionTexts
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderCompletionTexts.ToListAsync());
        }

        // GET: Admin/OrderCompletionTexts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderCompletionText = await _context.OrderCompletionTexts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderCompletionText == null)
            {
                return NotFound();
            }

            return View(orderCompletionText);
        }

        // GET: Admin/OrderCompletionTexts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/OrderCompletionTexts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PaymendType,Name,Content")] OrderCompletionText orderCompletionText)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderCompletionText);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderCompletionText);
        }

        // GET: Admin/OrderCompletionTexts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderCompletionText = await _context.OrderCompletionTexts.FindAsync(id);
            if (orderCompletionText == null)
            {
                return NotFound();
            }
            return View(orderCompletionText);
        }

        // POST: Admin/OrderCompletionTexts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PaymendType,Name,Content")] OrderCompletionText orderCompletionText)
        {
            if (id != orderCompletionText.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderCompletionText);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderCompletionTextExists(orderCompletionText.ID))
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
            return View(orderCompletionText);
        }

        // GET: Admin/OrderCompletionTexts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderCompletionText = await _context.OrderCompletionTexts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderCompletionText == null)
            {
                return NotFound();
            }

            return View(orderCompletionText);
        }

        // POST: Admin/OrderCompletionTexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderCompletionText = await _context.OrderCompletionTexts.FindAsync(id);
            _context.OrderCompletionTexts.Remove(orderCompletionText);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderCompletionTextExists(int id)
        {
            return _context.OrderCompletionTexts.Any(e => e.ID == id);
        }
    }
}
