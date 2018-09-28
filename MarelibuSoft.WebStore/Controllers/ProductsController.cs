using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Models.ViewModels;
using MarelibuSoft.WebStore.Common.Helpers;

namespace MarelibuSoft.WebStore.Areas.Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Store/Products
        public async Task<IActionResult> Index([FromQuery]int? categoryId, [FromQuery]int? categorySubId, [FromQuery]int? categoryDetailId)
        {
			string breadcrumelink = "/Products";
			string breadcrume = "/Products";
			var products = await _context.Products.Include(p => p.ImageList).Include(ca => ca.CategoryAssignments).ToListAsync();
			List<Product> filterProducts = new List<Product>();
			List<ProductThumbnailsViewModel> thubnails = new List<ProductThumbnailsViewModel>();
			var categoryAssignments = new List<CategoryAssignment>();
			if (categoryId != null)
			{
				var category = await _context.Categories.Where(c => c.ID == categoryId).SingleAsync();
				breadcrume += $"/{category.Name}";
				breadcrumelink += $"?categoryId={categoryId}"; 
				if(categorySubId != null)
				{
					var categorySub = await _context.CategorySubs.Where(c => c.ID == categorySubId).SingleAsync();
					breadcrume += $"/{categorySub.Name}";
					breadcrumelink += $"?categorySubId={categorySubId}";
					if (categoryDetailId != null)
					{
						var categoryDetail = await _context.CategoryDetails.Where(c => c.ID == categoryDetailId).SingleAsync();
						breadcrume += $"/{categoryDetail.Name}";
						breadcrumelink += $"?categoryDetailId={categoryDetailId}";
						categoryAssignments = await _context.CategoryAssignments.Where(ca => ca.CategoryID == categoryId && ca.CategorySubID == categorySubId && ca.CategoryDetailID == categoryDetailId).ToListAsync();
					}
					else {
						categoryAssignments = await _context.CategoryAssignments.Where(ca => ca.CategoryID == categoryId && ca.CategorySubID == categorySubId).ToListAsync();
					}
				}
				else
				{
					categoryAssignments = await _context.CategoryAssignments.Where(ca => ca.CategoryID == categoryId).ToListAsync();
				}

				if (categoryAssignments.Count > 0)
				{
					foreach (CategoryAssignment item in categoryAssignments)
					{
						var product = products.Where(p => p.ProductID == item.ProductID).FirstOrDefault();

						if (!filterProducts.Contains(product))
						{
							filterProducts.Add(product); 
						}
					}
				}
				else
				{
					filterProducts = products;
				}
			}
			if (products != null && filterProducts.Count == 0)
			{
				filterProducts = products;
			}

			foreach (Product item in filterProducts)
			{
				List<string> urls = new List<string>();
				string baseUnit = string.Empty;
				string secondPriceUnit = string.Empty;
				ProductThumbnailsViewModel vmProduct = new ProductThumbnailsViewModel();

				baseUnit = new UnitHelper(_context).GetUnitName(item.BasesUnitID);

				if (item.SecondBasePrice != 0.0M && item.SecondBaseUnit != 0)
				{
					string strUnit = new UnitHelper(_context).GetUnitName(item.SecondBaseUnit);
					secondPriceUnit = Math.Round( item.SecondBasePrice, 2).ToString() + " €/" + strUnit;
				}

				try
				{
					vmProduct.ProductID = item.ProductID;
					vmProduct.ProductNumber = item.ProductNumber;
					vmProduct.Price = item.Price;
					vmProduct.AvailableQuantity = item.AvailableQuantity;
					vmProduct.BasesUnit = baseUnit;
					vmProduct.BasesUnitID = item.BasesUnitID;
					vmProduct.Description = item.Description;
					vmProduct.MinimumPurchaseQuantity = item.MinimumPurchaseQuantity;
					vmProduct.Name = item.Name;
					vmProduct.ShippingTime = new ShippingPeriodHelper(_context).GetDescription(item.ShippingPeriod);
					vmProduct.Size = new SizeHelper(_context).GetName(item.Size);
					vmProduct.ShortDescription = item.ShortDescription;
					vmProduct.SecondPriceUnit = secondPriceUnit;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}

				foreach (ProductImage img in item.ImageList)
				{
					if (img.IsMainImage)
					{
						vmProduct.MainImageUrl = img.ImageUrl;
					}
					urls.Add( img.ImageUrl);
				}
				if (string.IsNullOrEmpty( vmProduct.MainImageUrl))
				{
					if (urls.Count > 0)
					{
						vmProduct.MainImageUrl = urls.First();
					}
					else
					{
						vmProduct.MainImageUrl = "noImage.svg";
					}
				}
				vmProduct.ImageUrls = urls;
				
				thubnails.Add(vmProduct);
			}
			ViewData["Breadcrumlink"] = breadcrumelink;
			ViewData["Breadcrum"] = breadcrume;
			return View(thubnails);
		}

		// GET: Store/Products/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
				.Include(p => p.ImageList)
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

			List<ProductImage> imgs = product.ImageList;

			List<string> imgUrls = new List<string>();
			foreach (ProductImage item in imgs)
			{
				imgUrls.Add(item.ImageUrl);
			}
			string mainImg = GetMainImageUrl(imgs);

			string secondPriceUnit = "";
			if (product.SecondBasePrice != 0.0M && product.SecondBaseUnit != 0)
			{
				string strUnit = new UnitHelper(_context).GetUnitName(product.SecondBaseUnit);
				secondPriceUnit = Math.Round( product.SecondBasePrice,2 ).ToString() + " €/" + strUnit;
			}

			string baseuint = new UnitHelper(_context).GetUnitName(product.BasesUnitID);

			ProductDetailViewModel dvm = new ProductDetailViewModel()
			{
				ProductID = product.ProductID,
				AvailableQuantity = product.AvailableQuantity,
				BasesUnit = baseuint,
				BasesUnitID = product.BasesUnitID,
				Description = product.Description,
				Price = product.Price,
				MinimumPurchaseQuantity = product.MinimumPurchaseQuantity,
				Name = product.Name,
				ProductNumber = product.ProductNumber,
				ShippingTime = new ShippingPeriodHelper(_context).GetDescription(product.ShippingPeriod),
				ShortDescription = product.ShortDescription,
				Size = new SizeHelper(_context).GetName(product.Size),
				ImageUrls = imgUrls,
				MainImageUrl = mainImg,
				SecondPriceUnit = secondPriceUnit
			};


			return View(dvm);
        }

        // GET: Store/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Store/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductNumber,Name,Description,Price,AvailableQuantity,MinimumPurchaseQuantity,BasesUnit,Size,ShippingTime,CategoryIDID,CategorySubID,CategoryDetailIDID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Store/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Store/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductNumber,Name,Description,Price,AvailableQuantity,MinimumPurchaseQuantity,BasesUnit,Size,ShippingTime,CategoryIDID,CategorySubID,CategoryDetailIDID")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            return View(product);
        }

        // GET: Store/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Store/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

		private string GetMainImageUrl(List<ProductImage> images)
		{
			string restult = "noImage.svg";

			foreach (ProductImage item in images)
			{
				if (item.IsMainImage)
				{
					restult = item.ImageUrl;
					break;
				}
			}

			if (restult.Equals("noImage.svg") && images.Count >0)
			{
				restult = images.First().ImageUrl;
			}
			return restult;
		}
    }
}
