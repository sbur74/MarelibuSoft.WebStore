using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Common.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using MarelibuSoft.WebStore.Generics;
using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using MarelibuSoft.WebStore.Common.Helpers;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private CultureInfo deDE = new CultureInfo("de-DE");

		public ProductsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(string searchString, int? page)
        {
			List<IndexProductViewModel> vms = new List<IndexProductViewModel>();
			ViewData["CurrentFilter"] = searchString;
			ViewData["Units"] = from u in _context.Units select u;
			ViewData["ShippingPriceType"] = from p in _context.ShippingPriceTypes select p;
			int pageSize = 5;

			var products = from p in _context.Products select p;
			if (!String.IsNullOrEmpty(searchString))
			{
				products = products.Where(p => p.Name.Contains(searchString));
			}

			foreach (var item in products)
			{
				IndexProductViewModel vm = new IndexProductViewModel()
				{					
					ProductID = item.ProductID,
					ProductNumber = item.ProductNumber,
					Price = item.Price,
					AvailableQuantity = Math.Round(item.AvailableQuantity,2),
					BasesUnit = new UnitHelper(_context).GetUnitName(item.BasesUnitID),
					Description = item.Description,
					Name = item.Name,
					ShortDescription = item.ShortDescription,
					ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(item.ShippingPriceType),
					ShippingTime = new ShippingPeriodHelper(_context).GetDescription(item.ShippingPeriod)
				};
				vms.Add(vm);
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
                .SingleOrDefaultAsync(m => m.ProductID == id);

			AdminProductViewModel vm = new AdminProductViewModel()
			{
				ProductID = product.ProductID,
				AvailableQuantity = Math.Round(product.AvailableQuantity,2).ToString(),
				BasesUnitID = product.BasesUnitID,
				BasesUnit = new UnitHelper(_context).GetUnitName(product.BasesUnitID),
				Description = product.Description,
				//GroupName = grpName,
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
				SecondBaseUnit = new UnitHelper(_context).GetUnitName(product.SecondBaseUnit),
				SecondBaseUnitID = product.SecondBaseUnit,
				ImageUrls = new ProductImageHelper(_context).GetUrls(product.ProductID),
				//CategoryID = product.CategoryID,
				//CategoryName = new CategoryRepository(_context).GetNameByID(product.CategoryID),
				//CategorySubID = product.CategorySubID,
				//CategorySubName = new CategorySubRepository(_context).GetNameByID(product.CategorySubID),
				//CategoryDetailID = product.CategoryDetailID,
				//CategoryDetailName = new CategoryDetailHelper(_context).GetNameByID(product.CategoryDetailID)
				ShippingPriceTypeID = product.ShippingPriceType,
				ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(product.ShippingPriceType)
			};
            if (product == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
			int actProductNumber = 0;
			var lastProd = _context.Products.LastOrDefault();
			if(lastProd != null)
			{
				actProductNumber = lastProd.ProductNumber + 1;
			}
			else
			{
				actProductNumber = 1000;
			}
			//ViewData["MainGroupID"] =  new SelectList(_context.Catagories, "CatagoryID", "Name");


			List<UnitViewModel> vmunits = new UnitHelper(_context).GetVmUnits();
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

			var vm = new AdminProductViewModel();
			vm.ProductNumber = actProductNumber;
			return View(vm) ;
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductNumber,Name,Description,Price,AvailableQuantity,MinimumPurchaseQuantity,BasesUnitID,SizeID,PeriodID,SecondBaseUnitID,SecondBasePrice,ShortDescription,CategoryID,CategorySubID,CategoryDetailID,ShippingPriceTypeID")] AdminProductViewModel vm)
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
				ShippingPriceType = vm.ShippingPriceTypeID
				//CategoryDetailID = vm.CategoryDetailID,
				//CategoryID = vm.CategoryID,
				//CategorySubID = vm.CategorySubID
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
				AvailableQuantity = Math.Round(product.AvailableQuantity,2).ToString(),
				Description = product.Description,
				MinimumPurchaseQuantity = Math.Round(product.MinimumPurchaseQuantity,2).ToString(),
				Name = product.Name,
				Price = Math.Round(product.Price,2).ToString(),
				ProductNumber = product.ProductNumber,
				PeriodID = product.ShippingPeriod,
				SizeID = product.Size,
				ShortDescription = product.ShortDescription,
				BasesUnitID = product.BasesUnitID,
				SecondBaseUnitID = product.SecondBaseUnit,
				SecondBasePrice = Math.Round(product.SecondBasePrice,2).ToString(),
				
				BasesUnit = new UnitHelper(_context).GetUnitName(product.BasesUnitID),
				Period = new ShippingPeriodHelper(_context).GetDescription(product.ShippingPeriod),
				SecondBaseUnit = new UnitHelper(_context).GetUnitName(product.SecondBaseUnit),
				Size = new SizeHelper(_context).GetName(product.Size),
				ImageUrls = new ProductImageHelper(_context).GetUrls(product.ProductID),
				ShippingPriceTypeID = product.ShippingPriceType,
				ShippingPriceTypeName = new ShippingPriceTypeHelper(_context).GetNameByID(product.ShippingPriceType)
			};

			List<UnitViewModel> vmunits = new UnitHelper(_context).GetVmUnits();
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

			return View(vm);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ProductID, [Bind("ProductID,ProductNumber,Name,Description,Price,AvailableQuantity,MinimumPurchaseQuantity,BasesUnitID,SizeID,PeriodID,ShortDescription,SecondBasePrice,SecondBaseUnitID,CategoryID,CategorySubID,CategoryDetailID,ShippingPriceTypeID ")] AdminProductViewModel vm)
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
				//CategoryDetailID =vm.CategoryDetailID,
				//CategoryID = vm.CategoryID,
				//CategorySubID = vm.CategorySubID
				ShippingPriceType = vm.ShippingPriceTypeID
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
