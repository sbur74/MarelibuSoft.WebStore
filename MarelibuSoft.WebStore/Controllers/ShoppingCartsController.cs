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
using MarelibuSoft.WebStore.Common.Statics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.Controllers
{
	public class ShoppingCartsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ILoggerFactory factory;
		private readonly ILogger logger;
		private ShoppingCartHelper cartHelper;

		public ShoppingCartsController(ApplicationDbContext context, ILoggerFactory loggerFactory)
		{
			_context = context;
			factory = loggerFactory;
			logger = factory.CreateLogger<ShoppingCartsController>();
			cartHelper = new ShoppingCartHelper(_context, factory.CreateLogger<ShoppingCartHelper>());
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
			decimal shipDefaultPrice = 0.0M;
			int countryDefault = 1;//Country 1 Deutschland
			int periodDefault = 1;			

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
				var shipping = await _context.ShippingAddresses.SingleOrDefaultAsync(c => c.CustomerID == shoppingCart.CustomerId && c.IsMainAddress);
				if (shipping == null)
				{
					return RedirectToAction("CustomerIndex", "ShippingAddresses", new { id = shoppingCart.CustomerId });
				}
				countryDefault = shipping.CountryID;
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

				if (periodDefault < product.ShippingPeriod)
				{
					periodDefault = product.ShippingPeriod;
				}

				var productShipPrice = await _context.ShippingPrices.SingleAsync(s => s.ShippingPriceTypeId == product.ShippingPriceType && s.CountryId == countryDefault);

				if (shipDefaultPrice < productShipPrice.Price) 
				{
					shipDefaultPrice = productShipPrice.Price;
					shipTypeDefault = productShipPrice.ShippingPriceTypeId;
				}

                //decimal baseprice = _context.Products.Where(p => p.ProductID.Equals(item.ProductID)).SingleOrDefault().Price;

                decimal baseprice = item.SellBasePrice; // immer den an der Warenkorbzeile nehmen

                decimal sekprice = Math.Round(product.SecondBasePrice, 2);

                if (item.SellActionItemId > 0)
                {
                    var sellActionItem = await _context.SellActionItems.SingleOrDefaultAsync(i => i.SellActionItemID == item.SellActionItemId);
                    if (sellActionItem != null)
                    {
                        var action = await _context.SellActions.FirstAsync(a => a.SellActionID == sellActionItem.SellActionID);
                        sekprice = Math.Round((sekprice - (sekprice * action.Percent) / 100), 2);
                    }
                }

                decimal pPrice = 0.0M;
				if (baseprice != 0.0M)
				{
					pPrice = baseprice * item.Quantity;
				}

				if (string.IsNullOrEmpty(path))
				{
					path = "noImage.svg";
				}
				string unit = new UnitHelper(_context, factory).GetUnitName(product.BasesUnitID);
				string sekunit = new UnitHelper(_context, factory).GetUnitName(product.SecondBaseUnit);

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
					ShortDescription = product.ShortDescription,
					MinimumPurchaseQuantity = Math.Round(product.MinimumPurchaseQuantity, 2),
					AvailableQuantity = Math.Round(product.AvailableQuantity, 2),
					ShoppingCartID = shoppingCart.ID,
                    SellActionItemId = item.SellActionItemId,
					SellBasePrice = Math.Round(item.SellBasePrice, 2),
					SellSekPrice = sekprice,
					SekUnit = sekunit,
                    SlugUrl = $"{item.ProductID}-{product.ProductNumber}-{FriendlyUrlHelper.ReplaceUmlaute(product.Name)}"
				};
				vmcLines.Add(cvml);
				total = total + pPrice;
			}

			ShippingPeriod shippingPeriod = await _context.ShpippingPeriods.SingleAsync(s => s.ShippingPeriodID == periodDefault);

			total = total + shipDefaultPrice;
			shipDefaultPrice = Math.Round(shipDefaultPrice, 2);
			var shippreise = await new ShippingPricesHelpers(_context).GetShippingPricesViewModels(shipTypeDefault);

			CartViewModel vm = new CartViewModel()
			{
				ID = shoppingCart.ID,
				Number = shoppingCart.Number,
				OrderId = shoppingCart.OrderId,
				Lines = vmcLines,
				Total = total,
				DefaultCountry = countryDefault,
				ShipPrices = shippreise,
				ShippingPeriodName = shippingPeriod.Decription
			};

			cartHelper.CheckAndRemove();

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
				if (baseprice != 0.0M)
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