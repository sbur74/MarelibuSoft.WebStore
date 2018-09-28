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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using MarelibuSoft.WebStore.Generics;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
		private IHostingEnvironment _environment;

		public ProductImagesController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
			_environment = env;
        }

        // GET: Admin/ProductImages
        public async Task<IActionResult> Index()
        {
            var imges = await _context.ProductImages.ToListAsync();
            return View(imges);
        }

        // GET: Admin/ProductImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .SingleOrDefaultAsync(m => m.ProductImageID == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // GET: Admin/ProductImages/Create
        public IActionResult Create(int? id)
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");

			ProductImage image = new ProductImage();
			if (id != null)
			{
				image.ProductID =(int) id;
				ViewData["SenderID"] = id;
			}
	
            return View(image);
        }

        // POST: Admin/ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductImageID,Name,IsMainImage,ImageUrl,ProductID")] ProductImage productImage)
        {
			

            if (ModelState.IsValid)
            {
				var files = HttpContext.Request.Form.Files;
				foreach (var image in files)
				{
					if (image != null && image.Length > 0)
					{
						var file = image;
						if (file.Length >0)
						{							
							try
							{
								UploadHelper helper = new UploadHelper(_environment);
								var names = await helper.FileUploadAsync(file, "images/store", true);
								productImage.ImageUrl = names.Filename;
								productImage.Name = names.Name;
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex);
							}
						}
					}
				}

				_context.Add(productImage);
                await _context.SaveChangesAsync();
				return Redirect("/Admin/Products/Details?id=" + productImage.ProductID);
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", productImage.ProductID);
            return View(productImage);
        }

        // GET: Admin/ProductImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages.SingleOrDefaultAsync(m => m.ProductImageID == id);
            if (productImage == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // POST: Admin/ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ProductImageID, [Bind("ProductImageID,Name,IsMainImage, ProductID")] ProductImage productImage)
        {
            if (ProductImageID != productImage.ProductImageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				try
				{
					var images = await _context.ProductImages.Where(i => i.ProductID == productImage.ProductID).ToListAsync();

					foreach (var item in images)
					{
						if (item.ProductImageID == productImage.ProductImageID)
						{
							item.IsMainImage = true;
						}
						else
						{
							item.IsMainImage = false;
						}
						_context.Entry(item).State = EntityState.Modified;
					}

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductImageExists(productImage.ProductImageID))
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
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // GET: Admin/ProductImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .SingleOrDefaultAsync(m => m.ProductImageID == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // POST: Admin/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImage = await _context.ProductImages.SingleOrDefaultAsync(m => m.ProductImageID == id);
			var helper = new UploadHelper(_environment);
			helper.DeleteFile("images/store", productImage.ImageUrl);
			_context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool ProductImageExists(int id)
        {
            return _context.ProductImages.Any(e => e.ProductImageID == id);
        }
    }
}
