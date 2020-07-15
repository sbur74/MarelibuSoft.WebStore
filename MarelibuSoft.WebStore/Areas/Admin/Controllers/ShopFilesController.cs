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
using MarelibuSoft.WebStore.Enums;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class ShopFilesController : Controller
    {
        private readonly ApplicationDbContext _context;
		private IWebHostEnvironment _environment;

        public ShopFilesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
			_environment = environment;
        }

        // GET: Admin/ShopFiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShopFiles.ToListAsync());
        }

        // GET: Admin/ShopFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopFile = await _context.ShopFiles
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shopFile == null)
            {
                return NotFound();
            }

            return View(shopFile);
        }

        // GET: Admin/ShopFiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ShopFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Filename,ShopFileType,IsActive")] ShopFile shopFile)
        {
            if (ModelState.IsValid)
            {
				var files = HttpContext.Request.Form.Files;
				if (files != null && files.Count > 0)
				{
					var file = files.First();
					var helper = new UploadHelper(_environment);
					var fnames = await helper.FileUploadAsync(file, "files", false);
					shopFile.Name = fnames.Name;
					shopFile.Filename = fnames.Filename;
				}
                _context.Add(shopFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopFile);
        }

        // GET: Admin/ShopFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopFile = await _context.ShopFiles.SingleOrDefaultAsync(m => m.ID == id);
            if (shopFile == null)
            {
                return NotFound();
            }
            return View(shopFile);
        }

        // POST: Admin/ShopFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Filename,ShopFileType,IsActive")] ShopFile shopFile)
        {
            if (id != shopFile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopFileExists(shopFile.ID))
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
            return View(shopFile);
        }

        // GET: Admin/ShopFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopFile = await _context.ShopFiles
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shopFile == null)
            {
                return NotFound();
            }

            return View(shopFile);
        }

        // POST: Admin/ShopFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopFile = await _context.ShopFiles.SingleOrDefaultAsync(m => m.ID == id);
			var helper = new UploadHelper(_environment);
			helper.DeleteFile("files", shopFile.Filename);
            _context.ShopFiles.Remove(shopFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopFileExists(int id)
        {
            return _context.ShopFiles.Any(e => e.ID == id);
        }
    }
}
