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
    public class BankAcccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BankAcccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/BankAcccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankAcccounts.ToListAsync());
        }

        // GET: Admin/BankAcccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAcccount = await _context.BankAcccounts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bankAcccount == null)
            {
                return NotFound();
            }

            return View(bankAcccount);
        }

        // GET: Admin/BankAcccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/BankAcccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccountOwner,Institute,Iban,SwiftBic")] BankAcccount bankAcccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankAcccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAcccount);
        }

        // GET: Admin/BankAcccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAcccount = await _context.BankAcccounts.FindAsync(id);
            if (bankAcccount == null)
            {
                return NotFound();
            }
            return View(bankAcccount);
        }

        // POST: Admin/BankAcccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AccountOwner,Institute,Iban,SwiftBic")] BankAcccount bankAcccount)
        {
            if (id != bankAcccount.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAcccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAcccountExists(bankAcccount.ID))
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
            return View(bankAcccount);
        }

        // GET: Admin/BankAcccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAcccount = await _context.BankAcccounts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bankAcccount == null)
            {
                return NotFound();
            }

            return View(bankAcccount);
        }

        // POST: Admin/BankAcccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankAcccount = await _context.BankAcccounts.FindAsync(id);
            _context.BankAcccounts.Remove(bankAcccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAcccountExists(int id)
        {
            return _context.BankAcccounts.Any(e => e.ID == id);
        }
    }
}
