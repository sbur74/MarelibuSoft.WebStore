using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;

namespace MarelibuSoft.WebStore.Controllers
{
    public class ShoppingCartLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartLinesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ShoppingCartLines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShoppingCartLines.Include(s => s.ShoppingCart);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCartLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartLine = await _context.ShoppingCartLines
                .Include(s => s.ShoppingCart)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCartLine == null)
            {
                return NotFound();
            }

            return View(shoppingCartLine);
        }

        // GET: ShoppingCartLines/Create
        public IActionResult Create()
        {
            ViewData["ShoppingCartID"] = new SelectList(_context.ShoppingCarts, "ID", "ID");
            return View();
        }

        // POST: ShoppingCartLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Position,ProductID,Quantity,Unit,ShoppingCartID")] ShoppingCartLine shoppingCartLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCartLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ShoppingCartID"] = new SelectList(_context.ShoppingCarts, "ID", "ID", shoppingCartLine.ShoppingCartID);
            return View(shoppingCartLine);
        }

        // GET: ShoppingCartLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartLine = await _context.ShoppingCartLines.SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCartLine == null)
            {
                return NotFound();
            }
            ViewData["ShoppingCartID"] = new SelectList(_context.ShoppingCarts, "ID", "ID", shoppingCartLine.ShoppingCartID);
            return View(shoppingCartLine);
        }

        // POST: ShoppingCartLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Position,ProductID,Quantity,Unit,ShoppingCartID")] ShoppingCartLine shoppingCartLine)
        {
            if (id != shoppingCartLine.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartLineExists(shoppingCartLine.ID))
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
            ViewData["ShoppingCartID"] = new SelectList(_context.ShoppingCarts, "ID", "ID", shoppingCartLine.ShoppingCartID);
            return View(shoppingCartLine);
        }

        // GET: ShoppingCartLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartLine = await _context.ShoppingCartLines
                .Include(s => s.ShoppingCart)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCartLine == null)
            {
                return NotFound();
            }

            return View(shoppingCartLine);
        }

        // POST: ShoppingCartLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCartLine = await _context.ShoppingCartLines.SingleOrDefaultAsync(m => m.ID == id);
            _context.ShoppingCartLines.Remove(shoppingCartLine);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ShoppingCartLineExists(int id)
        {
            return _context.ShoppingCartLines.Any(e => e.ID == id);
        }
    }
}
