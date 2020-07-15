using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Common.Helpers;
using MarelibuSoft.WebStore.Common.Statics;
using MarelibuSoft.WebStore.Common.ViewModels;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;
		private readonly ILoggerFactory factory;
		private CultureInfo deDE = new CultureInfo("de-DE");

		public ProductsController(ApplicationDbContext context 
			//ILogger<ProductsController> logger, 
			//ILoggerFactory loggerFactory
			)
        {
            _context = context;
			//_logger = logger;
			//factory = loggerFactory;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(string searchString)
        {
			List<IndexProductViewModel> vms = new List<IndexProductViewModel>();
			ViewData["CurrentFilter"] = searchString;

			var products =  _context.Products.OrderByDescending(p => p.ProductNumber).ToList();
			

			foreach (var item in products)
			{
				string mainImgStr = string.Empty;

				try
				{
					var mainImg = await _context.ProductImages.SingleAsync(i => i.ProductID == item.ProductID && i.IsMainImage);
					mainImgStr = mainImg.ImageUrl;
				}
				catch (Exception e)
				{
					//_logger.LogError(e, "Fehler, beim ermittel eines Hauptatriklebildes!");
					mainImgStr = "noImage.svg";
					Console.WriteLine(e);
				}

				 
				if (string.IsNullOrWhiteSpace(mainImgStr)) mainImgStr = "noImage.svg";

				IndexProductViewModel vm = new IndexProductViewModel()
				{					
					ProductID = item.ProductID,
					ProductNumber = item.ProductNumber,
					Price = item.Price,
					AvailableQuantity = Math.Round(item.AvailableQuantity,2),
					BasesUnit = new UnitHelper(_context, factory).GetUnitName(item.BasesUnitID),
					Description = item.Description,
					Name = item.Name,
					ShortDescription = item.ShortDescription,
					ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(item.ShippingPriceType),
					ShippingTime = new ShippingPeriodHelper(_context).GetDescription(item.ShippingPeriod),
					IsActive = item.IsActive,
					MainImage = mainImgStr,
                    Slug = $"{item.ProductID}-{item.ProductNumber}-{FriendlyUrlHelper.ReplaceUmlaute(item.Name)}"
                };

                if (!String.IsNullOrEmpty(searchString))
                {
                    if(item.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                    || item.ProductNumber.ToString().Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        vms.Add(vm);
                    }
                }
                else
                {
                    vms.Add(vm);
                }
			}

			//IQueryable<IndexProductViewModel> queryable = vms.AsQueryable();

			//return View(await PaginatedList<IndexProductViewModel>.CeateAsync(queryable.AsNoTracking(), page ?? 1, pageSize));

			return View(vms);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(v => v.ProductVariants)
                .ThenInclude(o => o.Options)
                .SingleOrDefaultAsync(m => m.ProductID == id);

			AdminProductViewModel vm = new AdminProductViewModel()
			{
				ProductID = product.ProductID,
				AvailableQuantity = Math.Round(product.AvailableQuantity,2).ToString(),
				BasesUnitID = product.BasesUnitID,
				BasesUnit = new UnitHelper(_context, factory).GetUnitName(product.BasesUnitID),
				Description = product.Description,
				IsActive = product.IsActive,
				Name = product.Name,
				//MainGroupName = mGrpName,
				MinimumPurchaseQuantity = Math.Round( product.MinimumPurchaseQuantity,2).ToString(),
				Price = Math.Round(product.Price,2).ToString(),
				ProductNumber = product.ProductNumber,
				Period = new ShippingPeriodHelper(_context).GetDescription(product.ShippingPeriod),
				PeriodID = product.ShippingPeriod,
				Size = new SizeHelper(_context).GetName(product.Size),
				SizeID = product.Size,
				ShortDescription = product.ShortDescription,
				SecondBasePrice = Math.Round(product.SecondBasePrice,2).ToString(),
				SecondBaseUnit = new UnitHelper(_context, factory).GetUnitName(product.SecondBaseUnit),
				SecondBaseUnitID = product.SecondBaseUnit,
				ImageUrls = new ProductImageHelper(_context, factory).GetUrls(product.ProductID),
				//CategoryID = product.CategoryID,
				//CategoryName = new CategoryRepository(_context).GetNameByID(product.CategoryID),
				//CategorySubID = product.CategorySubID,
				//CategorySubName = new CategorySubRepository(_context).GetNameByID(product.CategorySubID),
				//CategoryDetailID = product.CategoryDetailID,
				//CategoryDetailName = new CategoryDetailHelper(_context).GetNameByID(product.CategoryDetailID)
				ShippingPriceTypeID = product.ShippingPriceType,
				ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(product.ShippingPriceType),
				SeoDescription = product.SeoDescription,
				SeoKeywords = product.SeoKeywords,
                Variants = product.ProductVariants,
                IsShowTextVariant = product.IsShowTextVariant,
                TextVariantTitel = product.TextVariantTitel
			};
            if (product == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
			int actProductNumber = 0;
			var lastProd = await _context.Products.LastOrDefaultAsync();
			if(lastProd != null)
			{
				actProductNumber = lastProd.ProductNumber + 1;
			}
			else
			{
				actProductNumber = 1000;
			}
			//ViewData["MainGroupID"] =  new SelectList(_context.Catagories, "CatagoryID", "Name");


			List<UnitViewModel> vmunits = new UnitHelper(_context, factory).GetVmUnits();
			List<SizeViewModel> vwsizes = new SizeHelper(_context).GetVmSizes();
			List<ShippingPeriodViewModel> periods = new ShippingPeriodHelper(_context).GetVmShippingPeriods();
			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();
			List<SelectItemViewModel> shippingPriceTypes = new ShippingPriceTypeHelper(_context).GetVmList();

			ViewData["BaseUnit"] = new SelectList(vmunits, "UnitID", "Name" );
			ViewData["Size"] = new SelectList(vwsizes, "ID", "Name");
			ViewData["SeconedUnit"] = new SelectList(vmunits, "UnitID", "Name");
			ViewData["Periods"] = new SelectList(periods, "ID", "Value");
			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");
			ViewData["ShippingPriceTypeID"] = new SelectList(shippingPriceTypes, "ID", "Name");
            ViewData["Behave"] = "Create";

            Product product = new Product { ProductNumber = actProductNumber };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var vm = new AdminProductViewModel();
            vm.ProductID = product.ProductID;
			vm.ProductNumber = product.ProductNumber;
			return View("Edit", vm) ;
        }

		// GET: Admin/Products/Edit/5
		public async Task<IActionResult> CreateCopy(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products.Include(v => v.ProductVariants).SingleOrDefaultAsync(m => m.ProductID == id);
			if (product == null)
			{
				return NotFound();
			}

			int actProductNumber = 0;
			var lastProd = _context.Products.LastOrDefault();
			if (lastProd != null)
			{
				actProductNumber = lastProd.ProductNumber + 1;
			}

            Product cproduct = new Product { ProductNumber = actProductNumber, ProductVariants = new List<ProductVariant>() };

            var variants = product.ProductVariants;

            foreach (ProductVariant variant in variants)
            {
                var cvt = new ProductVariant { Name = variant.Name, OptionName = variant.OptionName, Options = new List<ProductVariantOption>() };                var options = await _context.ProductVariantOptions.Where(o => o.ProductVariantID == variant.ID).ToListAsync();
                foreach (ProductVariantOption option in variant.Options)
                {
                    var copt = new ProductVariantOption
                    {
                        IsNotShown = option.IsNotShown, Option = option.Option, Price = option.Price, Quantity = option.Quantity
                    };
                    cvt.Options.Add(copt);
                }
                cproduct.ProductVariants.Add(cvt);
            }

            await _context.Products.AddAsync(cproduct);
            await _context.SaveChangesAsync();            

			AdminProductViewModel vm = new AdminProductViewModel()
			{
				ProductID = cproduct.ProductID,
				AvailableQuantity = Math.Round(product.AvailableQuantity, 2).ToString(),
				Description = product.Description,
				MinimumPurchaseQuantity = Math.Round(product.MinimumPurchaseQuantity, 2).ToString(),
				Name = "KOPIE - " + product.Name,
				Price = Math.Round(product.Price, 2).ToString(),
				ProductNumber = actProductNumber,
				PeriodID = product.ShippingPeriod,
				SizeID = product.Size,
				ShortDescription = product.ShortDescription,
				BasesUnitID = product.BasesUnitID,
				SecondBaseUnitID = product.SecondBaseUnit,
				SecondBasePrice = Math.Round(product.SecondBasePrice, 2).ToString(),
				IsActive = product.IsActive,
                IsShowTextVariant = product.IsShowTextVariant,
                TextVariantTitel = product.TextVariantTitel,
				BasesUnit = new UnitHelper(_context, factory).GetUnitName(product.BasesUnitID),
				Period = new ShippingPeriodHelper(_context).GetDescription(product.ShippingPeriod),
				SecondBaseUnit = new UnitHelper(_context, factory).GetUnitName(product.SecondBaseUnit),
				Size = new SizeHelper(_context).GetName(product.Size),
				ImageUrls = new ProductImageHelper(_context, factory).GetUrls(product.ProductID),
				ShippingPriceTypeID = product.ShippingPriceType,
				ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(product.ShippingPriceType),
				SeoDescription = product.SeoDescription,
				SeoKeywords = product.SeoKeywords,
                Variants = await _context.ProductVariants.Include(v => v.Options).Where(v => v.ProductId == cproduct.ProductID).ToListAsync()
			};

			List<UnitViewModel> vmunits = new UnitHelper(_context, factory).GetVmUnits();
			List<SizeViewModel> vwsizes = new SizeHelper(_context).GetVmSizes();
			List<ShippingPeriodViewModel> periods = new ShippingPeriodHelper(_context).GetVmShippingPeriods();
			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();
			List<SelectItemViewModel> shippingPriceTypes = new ShippingPriceTypeHelper(_context).GetVmList();

			ViewData["BaseUnit"] = new SelectList(vmunits, "UnitID", "Name");
			ViewData["Size"] = new SelectList(vwsizes, "ID", "Name");
			ViewData["SeconedUnit"] = new SelectList(vmunits, "UnitID", "Name");
			ViewData["Periods"] = new SelectList(periods, "ID", "Value");
			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");
			ViewData["ShippingPriceTypeID"] = new SelectList(shippingPriceTypes, "ID", "Name");
            ViewData["Behave"] = "Copy";

            return View("Edit", vm);
		}


		// POST: Admin/Products/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "ProductID,ProductNumber,Name,Description,Price,AvailableQuantity," +
                "MinimumPurchaseQuantity,BasesUnitID,SizeID,PeriodID,SecondBaseUnitID," +
                "SecondBasePrice,ShortDescription,CategoryID,CategorySubID,CategoryDetailID," +
                "ShippingPriceTypeID,IsActive,SeoDescription,SeoKeywords,IsShowTextVariant," +
                "TextVariantTitel, Variants")] AdminProductViewModel vm)
       {
			
			
			Product product = new Product()
			{
				ProductID = vm.ProductID,
				AvailableQuantity = StaticDecimalHelper.PaseString(vm.AvailableQuantity),
				Description = vm.Description,
				MinimumPurchaseQuantity = StaticDecimalHelper.PaseString(vm.MinimumPurchaseQuantity),
				Name = vm.Name,
				Price = StaticDecimalHelper.PaseString(vm.Price),
				ProductNumber = vm.ProductNumber,
				ShippingPeriod = vm.PeriodID,
				Size = vm.SizeID,
				ShortDescription = vm.ShortDescription,
				SecondBasePrice = StaticDecimalHelper.PaseString(vm.SecondBasePrice),
				SecondBaseUnit = vm.SecondBaseUnitID,
				BasesUnitID = vm.BasesUnitID,
				ShippingPriceType = vm.ShippingPriceTypeID,
				IsActive = vm.IsActive,
				SeoDescription = vm.SeoDescription,
				SeoKeywords = vm.SeoKeywords,
                PublishedOn = DateTime.Now,
                IsShowTextVariant = vm.IsShowTextVariant,
                ProductVariants = vm.Variants,
                TextVariantTitel = vm.TextVariantTitel
			};

			if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
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

            AdminProductViewModel vm = new AdminProductViewModel() {
                ProductID = product.ProductID,
                AvailableQuantity = Math.Round(product.AvailableQuantity, 2).ToString(),
                Description = product.Description,
                MinimumPurchaseQuantity = Math.Round(product.MinimumPurchaseQuantity, 2).ToString(),
                Name = product.Name,
                Price = Math.Round(product.Price, 2).ToString(),
                ProductNumber = product.ProductNumber,
                PeriodID = product.ShippingPeriod,
                SizeID = product.Size,
                ShortDescription = product.ShortDescription,
                BasesUnitID = product.BasesUnitID,
                SecondBaseUnitID = product.SecondBaseUnit,
                SecondBasePrice = Math.Round(product.SecondBasePrice, 2).ToString(),
                IsActive = product.IsActive,
                BasesUnit = new UnitHelper(_context, factory).GetUnitName(product.BasesUnitID),
                Period = new ShippingPeriodHelper(_context).GetDescription(product.ShippingPeriod),
                SecondBaseUnit = new UnitHelper(_context, factory).GetUnitName(product.SecondBaseUnit),
                Size = new SizeHelper(_context).GetName(product.Size),
                ImageUrls = new ProductImageHelper(_context, factory).GetUrls(product.ProductID),
                ShippingPriceTypeID = product.ShippingPriceType,
                ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(product.ShippingPriceType),
                SeoDescription = product.SeoDescription,
                SeoKeywords = product.SeoKeywords,
                IsShowTextVariant = product.IsShowTextVariant,
                TextVariantTitel = product.TextVariantTitel,
                Variants = await _context.ProductVariants.Where(pv => pv.ProductId == product.ProductID).Include(o => o.Options).ToListAsync()
            };

			List<UnitViewModel> vmunits = new UnitHelper(_context, factory).GetVmUnits();
			List<SizeViewModel> vwsizes = new SizeHelper(_context).GetVmSizes();
			List<ShippingPeriodViewModel> periods = new ShippingPeriodHelper(_context).GetVmShippingPeriods();
			List<SelectItemViewModel> catvms = new CategoryHelper(_context).GetVmList();
			List<SelectItemViewModel> catsubvms = new CategorySubHelper(_context).GetVmList();
			List<SelectItemViewModel> catdeatailvms = new CategoryDetailHelper(_context).GetVmList();
			List<SelectItemViewModel> shippingPriceTypes = new ShippingPriceTypeHelper(_context).GetVmList();

			ViewData["BaseUnit"] = new SelectList(vmunits, "UnitID", "Name");
			ViewData["Size"] = new SelectList(vwsizes, "ID", "Name");
			ViewData["SeconedUnit"] = new SelectList(vmunits, "UnitID", "Name");
			ViewData["Periods"] = new SelectList(periods, "ID", "Value");
			ViewData["CategoryID"] = new SelectList(catvms, "ID", "Name");
			ViewData["CategorySubID"] = new SelectList(catsubvms, "ID", "Name");
			ViewData["CategoryDetailID"] = new SelectList(catdeatailvms, "ID", "Name");
			ViewData["ShippingPriceTypeID"] = new SelectList(shippingPriceTypes, "ID", "Name");
            ViewData["Behave"] = "Edit";

            return View(vm);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ProductID, 
            [Bind(
                "ProductID," +
                "ProductNumber,Name,Description,Price,AvailableQuantity," +
                "MinimumPurchaseQuantity,BasesUnitID,SizeID,PeriodID," +
                "ShortDescription,SecondBasePrice,SecondBaseUnitID,CategoryID,CategorySubID,CategoryDetailID," +
                "ShippingPriceTypeID,IsActive,SeoDescription,SeoKeywords, IsShowTextVariant, TextVariantTitel, Variants")] 
                AdminProductViewModel vm)
        {
			Product product = new Product() {
				ProductID = vm.ProductID,
				ProductNumber = vm.ProductNumber,
				Price = StaticDecimalHelper.PaseString( vm.Price),
				Size = vm.SizeID,
				AvailableQuantity = StaticDecimalHelper.PaseString(vm.AvailableQuantity),
				BasesUnitID = vm.BasesUnitID,
				Description = vm.Description,
				MinimumPurchaseQuantity = StaticDecimalHelper.PaseString( vm.MinimumPurchaseQuantity),
				Name = vm.Name,
				ShippingPeriod = vm.PeriodID,
				ShortDescription = vm.ShortDescription,
				SecondBasePrice = StaticDecimalHelper.PaseString( vm.SecondBasePrice),
				SecondBaseUnit = vm.SecondBaseUnitID,
				IsActive = vm.IsActive,
				ShippingPriceType = vm.ShippingPriceTypeID,
				SeoDescription = vm.SeoDescription,
				SeoKeywords = vm.SeoKeywords,
                //PublishedOn = DateTime.Now,
                IsShowTextVariant = vm.IsShowTextVariant,
                TextVariantTitel = vm.TextVariantTitel,
                ProductVariants = vm.Variants
            };

			if (ProductID != vm.ProductID)
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

        // GET: Admin/Products/Delete/5
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

        // POST: Admin/Products/Delete/5
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
    }
}
