using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Hosting;
using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class PaymendsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private IHostingEnvironment _environment;

		public PaymendsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
			_environment = environment;
        }

        // GET: Admin/Paymends
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paymends.ToListAsync());
        }

        // GET: Admin/Paymends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymend = await _context.Paymends
                .SingleOrDefaultAsync(m => m.ID == id);
            if (paymend == null)
            {
                return NotFound();
            }

            return View(paymend);
        }

        // GET: Admin/Paymends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Paymends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LogoUrl,IsActive,PaymendType")] Paymend paymend)
        {
            if (ModelState.IsValid)
            {
				var files = HttpContext.Request.Form.Files;
				foreach (var item in files)
				{
					var names = await new UploadHelper(_environment).FileUploadAsync(item, "images/paymend",false);
					paymend.LogoUrl = names.Filename;
				}
				_context.Add(paymend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymend);
        }

        // GET: Admin/Paymends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymend = await _context.Paymends.SingleOrDefaultAsync(m => m.ID == id);
            if (paymend == null)
            {
                return NotFound();
            }
            return View(paymend);
        }

        // POST: Admin/Paymends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LogoUrl,IsActive,PaymendType")] Paymend paymend)
        {
            if (id != paymend.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymendExists(paymend.ID))
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
            return View(paymend);
        }

        // GET: Admin/Paymends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymend = await _context.Paymends
                .SingleOrDefaultAsync(m => m.ID == id);
            if (paymend == null)
            {
                return NotFound();
            }

            return View(paymend);
        }

        // POST: Admin/Paymends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymend = await _context.Paymends.SingleOrDefaultAsync(m => m.ID == id);
			var helper = new UploadHelper(_environment);
			helper.DeleteFile("images/paymend", paymend.LogoUrl);
            _context.Paymends.Remove(paymend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymendExists(int id)
        {
            return _context.Paymends.Any(e => e.ID == id);
        }
    }
}
