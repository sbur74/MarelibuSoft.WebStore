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
using Microsoft.AspNetCore.Http;

namespace MarelibuSoft.WebStore.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShoppingCarts.ToListAsync());
        }

		// GET: ShoppingCarts/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			var sessioncart = HttpContext.Session.GetString("ShoppingCartId");
			int shipTypeDefault = 1; //Type 1 = kleines Paket
			int countryDefault = 1;//Country 1 Deutschland

			if (id == null)
			{
				return NotFound();
			}

			var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.ID == id);
			
			if (shoppingCart == null)
			{
				return NotFound();
			}

			

			if(shoppingCart.CustomerId != Guid.Empty)
			{
				var customer = _context.Customers.Single(c => c.CustomerID == shoppingCart.CustomerId);
				countryDefault = customer.CountryId;
			}

			decimal total = 0.0M;
			var lines = _context.ShoppingCartLines.Where(l => l.ShoppingCartID.Equals(shoppingCart.ID));
			List<CartLineViewModel> vmcLines = new List<CartLineViewModel>();
			foreach (var item in lines)
			{
				string path = string.Empty;
				try
				{
					path = _context.ProductImages.Where(i => i.ProductID.Equals(item.ProductID) && i.IsMainImage).SingleOrDefault().ImageUrl;
				}
				catch (Exception)
				{
					path = "noImage.svg";
				}
				var product = _context.Products.Where(p => p.ProductID.Equals(item.ProductID)).SingleOrDefault();

				if (shipTypeDefault < product.ShippingPriceType) 
				{
					shipTypeDefault = product.ShippingPriceType;
				}

				decimal baseprice = _context.Products.Where(p => p.ProductID.Equals(item.ProductID)).SingleOrDefault().Price;

				decimal pPrice = 0.0M;
				if (baseprice != 0.0M)
				{
					pPrice = baseprice * item.Quantity;
				}

				if (string.IsNullOrEmpty(path))
				{
					path = "noImage.svg";
				}
				string unit = new UnitHelper(_context).GetUnitName(product.BasesUnitID);

				CartLineViewModel cvml = new CartLineViewModel()
				{
					ID = item.ID,
					CartID = item.ShoppingCartID,
					ImgPath = path,
					Position = item.Position,
					PosPrice = pPrice,
					Quantity = item.Quantity,
					ProductID = item.ProductID,
					Unit = unit,
					UnitID = item.UnitID,
					ProductName = product.Name,
					ProductNo = product.ProductNumber.ToString(),
					MinimumPurchaseQuantity = product.MinimumPurchaseQuantity,
					AvailableQuantity = product.AvailableQuantity,
					ShoppingCartID = shoppingCart.ID,
					SellBasePrice = item.SellBasePrice
				};
				vmcLines.Add(cvml);
				total = total + pPrice;
			}

			ShippingPrice defaultPrice = _context.ShippingPrices.Single(s => s.ShippingPriceTypeId == shipTypeDefault && s.CountryId == countryDefault);

			total = total + defaultPrice.Price;
			defaultPrice.Price = Math.Round(defaultPrice.Price, 2);
			ViewData["ShippingPrice"] = defaultPrice;

			CartViewModel vm = new CartViewModel()
			{
				ID = shoppingCart.ID,
				Number = shoppingCart.Number,
				OrderId = shoppingCart.OrderId,
				Lines = vmcLines,
				Total = total
			};


			return View(vm);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Number,CustomerId,GustId,OrderId")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                shoppingCart.ID = Guid.NewGuid();
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

			var lines = _context.ShoppingCartLines.Where(l => l.ShoppingCartID.Equals(shoppingCart.ID));
			List<CartLineViewModel> vmcLines = new List<CartLineViewModel>();
			foreach (var item in lines)
			{
				string path = _context.ProductImages.Where(i => i.ProductID.Equals(item.ProductID) && i.IsMainImage).SingleOrDefault().ImageUrl;
				var product = _context.Products.Where(p => p.ProductID.Equals(item.ProductID)).SingleOrDefault();

				decimal baseprice = _context.Products.Where(p => p.ProductID.Equals(item.ProductID)).SingleOrDefault().Price;

				decimal pPrice = 0.0M;
				if ( baseprice != 0.0M)
				{
					pPrice = baseprice * item.Quantity;
				}

				if (path == null || path.Length == 0)
				{
					path = "noImage.svg";
				}

				string unit = _context.Units.Where(u => u.UnitID == item.UnitID).Single().Name;

				CartLineViewModel cvml = new CartLineViewModel()
				{
					ID = item.ID,
					CartID = item.ShoppingCartID,
					ImgPath = path,
					Position = item.Position,
					PosPrice = pPrice,
					Quantity = item.Quantity,
					ProductID = item.ProductID,
					Unit = unit,
					ProductName = product.Name,
					ProductNo = product.ProductNumber.ToString(),
					ShoppingCartID = shoppingCart.ID
				};
				vmcLines.Add(cvml);
			}

			CartViewModel vm = new CartViewModel()
			{
				ID = shoppingCart.ID,
				Number = shoppingCart.Number,
				OrderId = shoppingCart.OrderId,
				Lines = vmcLines
			};


            return View(vm);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Number,OrderId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.ID))
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
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(m => m.ID == id);
            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ShoppingCartExists(Guid id)
        {
            return _context.ShoppingCarts.Any(e => e.ID == id);
        }
    }
}
