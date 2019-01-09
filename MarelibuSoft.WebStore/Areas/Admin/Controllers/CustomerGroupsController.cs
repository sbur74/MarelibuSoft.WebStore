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
    public class CustomerGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CustomerGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerGroups.ToListAsync());
        }

        // GET: Admin/CustomerGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerGroup = await _context.CustomerGroups
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customerGroup == null)
            {
                return NotFound();
            }

            return View(customerGroup);
        }

        // GET: Admin/CustomerGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CustomerGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Rabatt")] CustomerGroup customerGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerGroup);
        }

        // GET: Admin/CustomerGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerGroup = await _context.CustomerGroups.FindAsync(id);
            if (customerGroup == null)
            {
                return NotFound();
            }
            return View(customerGroup);
        }

        // POST: Admin/CustomerGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Rabatt")] CustomerGroup customerGroup)
        {
            if (id != customerGroup.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerGroupExists(customerGroup.ID))
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
            return View(customerGroup);
        }

        // GET: Admin/CustomerGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerGroup = await _context.CustomerGroups
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customerGroup == null)
            {
                return NotFound();
            }

            return View(customerGroup);
        }

        // POST: Admin/CustomerGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerGroup = await _context.CustomerGroups.FindAsync(id);
            _context.CustomerGroups.Remove(customerGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerGroupExists(int id)
        {
            return _context.CustomerGroups.Any(e => e.ID == id);
        }
    }
}
