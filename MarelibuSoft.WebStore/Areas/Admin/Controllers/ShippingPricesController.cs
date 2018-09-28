using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Common.Helpers;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingPricesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingPricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ShippingPrices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShippingPrices.Include(s => s.ShippingPriceType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ShippingPrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPrice = await _context.ShippingPrices
                .Include(s => s.ShippingPriceType)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shippingPrice == null)
            {
                return NotFound();
            }

            return View(shippingPrice);
        }

        // GET: Admin/ShippingPrices/Create
        public IActionResult Create()
        {
            ViewData["ShippingPriceTypeId"] = new SelectList(_context.ShippingPriceTypes, "ID", "Name");
			ViewData["CountryId"] = new SelectList(_context.Countries, "ID", "Name");
            return View();
        }

        // POST: Admin/ShippingPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,CountryId,ShippingPriceTypeId")] ShippingPrice shippingPrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShippingPriceTypeId"] = new SelectList(_context.ShippingPriceTypes, "ID", "Name", shippingPrice.ShippingPriceTypeId);
            return View(shippingPrice);
        }

        // GET: Admin/ShippingPrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPrice = await _context.ShippingPrices.SingleOrDefaultAsync(m => m.ID == id);
            if (shippingPrice == null)
            {
                return NotFound();
            }
            ViewData["ShippingPriceTypeId"] = new SelectList(_context.ShippingPriceTypes, "ID", "Name", shippingPrice.ShippingPriceTypeId);

			ViewData["CountryID"] = new SelectList(new CountryHelper(_context).GetVmList(shippingPrice.CountryId), "ID","Name");
            return View(shippingPrice);
        }

        // POST: Admin/ShippingPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,CountryId,ShippingPriceTypeId")] ShippingPrice shippingPrice)
        {
            if (id != shippingPrice.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingPriceExists(shippingPrice.ID))
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
            ViewData["ShippingPriceTypeId"] = new SelectList(_context.ShippingPriceTypes, "ID", "ID", shippingPrice.ShippingPriceTypeId);
            return View(shippingPrice);
        }

        // GET: Admin/ShippingPrices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPrice = await _context.ShippingPrices
                .Include(s => s.ShippingPriceType)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shippingPrice == null)
            {
                return NotFound();
            }

            return View(shippingPrice);
        }

        // POST: Admin/ShippingPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippingPrice = await _context.ShippingPrices.SingleOrDefaultAsync(m => m.ID == id);
            _context.ShippingPrices.Remove(shippingPrice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingPriceExists(int id)
        {
            return _context.ShippingPrices.Any(e => e.ID == id);
        }
    }
}
