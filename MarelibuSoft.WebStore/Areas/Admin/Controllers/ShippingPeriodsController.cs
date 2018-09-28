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
	public class ShippingPeriodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingPeriodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ShippingPeriods
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShpippingPeriods.ToListAsync());
        }

        // GET: Admin/ShippingPeriods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPeriod = await _context.ShpippingPeriods
                .SingleOrDefaultAsync(m => m.ShippingPeriodID == id);
            if (shippingPeriod == null)
            {
                return NotFound();
            }

            return View(shippingPeriod);
        }

        // GET: Admin/ShippingPeriods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ShippingPeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShippingPeriodID,Decription")] ShippingPeriod shippingPeriod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingPeriod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippingPeriod);
        }

        // GET: Admin/ShippingPeriods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPeriod = await _context.ShpippingPeriods.SingleOrDefaultAsync(m => m.ShippingPeriodID == id);
            if (shippingPeriod == null)
            {
                return NotFound();
            }
            return View(shippingPeriod);
        }

        // POST: Admin/ShippingPeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShippingPeriodID,Decription")] ShippingPeriod shippingPeriod)
        {
            if (id != shippingPeriod.ShippingPeriodID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingPeriod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingPeriodExists(shippingPeriod.ShippingPeriodID))
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
            return View(shippingPeriod);
        }

        // GET: Admin/ShippingPeriods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingPeriod = await _context.ShpippingPeriods
                .SingleOrDefaultAsync(m => m.ShippingPeriodID == id);
            if (shippingPeriod == null)
            {
                return NotFound();
            }

            return View(shippingPeriod);
        }

        // POST: Admin/ShippingPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippingPeriod = await _context.ShpippingPeriods.SingleOrDefaultAsync(m => m.ShippingPeriodID == id);
            _context.ShpippingPeriods.Remove(shippingPeriod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingPeriodExists(int id)
        {
            return _context.ShpippingPeriods.Any(e => e.ShippingPeriodID == id);
        }
    }
}
