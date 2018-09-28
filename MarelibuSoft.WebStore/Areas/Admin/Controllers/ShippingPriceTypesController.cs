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
    public class ShippingPriceTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingPriceTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ShippingPriceTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShippingPriceTypes.ToListAsync());
        }

        // GET: Admin/ShippingPriceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPriceType = await _context.ShippingPriceTypes
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shippingPriceType == null)
            {
                return NotFound();
            }

            return View(shippingPriceType);
        }

        // GET: Admin/ShippingPriceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ShippingPriceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] ShippingPriceType shippingPriceType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingPriceType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippingPriceType);
        }

        // GET: Admin/ShippingPriceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPriceType = await _context.ShippingPriceTypes.SingleOrDefaultAsync(m => m.ID == id);
            if (shippingPriceType == null)
            {
                return NotFound();
            }
            return View(shippingPriceType);
        }

        // POST: Admin/ShippingPriceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] ShippingPriceType shippingPriceType)
        {
            if (id != shippingPriceType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingPriceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingPriceTypeExists(shippingPriceType.ID))
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
            return View(shippingPriceType);
        }

        // GET: Admin/ShippingPriceTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPriceType = await _context.ShippingPriceTypes
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shippingPriceType == null)
            {
                return NotFound();
            }

            return View(shippingPriceType);
        }

        // POST: Admin/ShippingPriceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippingPriceType = await _context.ShippingPriceTypes.SingleOrDefaultAsync(m => m.ID == id);
            _context.ShippingPriceTypes.Remove(shippingPriceType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingPriceTypeExists(int id)
        {
            return _context.ShippingPriceTypes.Any(e => e.ID == id);
        }
    }
}
