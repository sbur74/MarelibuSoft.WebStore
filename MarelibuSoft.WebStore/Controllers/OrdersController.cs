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
using MarelibuSoft.WebStore.Common.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MarelibuSoft.WebStore.Services;

namespace MarelibuSoft.WebStore.Controllers
{
	[Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger _logger;
		private readonly IEmailSender _emailSender;

		public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser>userManager, ILogger<OrdersController>logger, IEmailSender emailSender)
        {
            _context = context;
			_userManager = userManager;
			_logger = logger;
			_emailSender = emailSender;
        }

        // GET: Orders
		public async Task<IActionResult> WeHaveYourOrder(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}


			WeHaveYourOrderViewModel viewModel = await GetViewModel((Guid)id); 
			
			return View(viewModel);
		}
		// GET: Orders/Create
		public async Task<IActionResult> Create(Guid? id)
        {
			OrderCreateViewModel vm = new OrderCreateViewModel();
			List<ShippingAddress> addresses = new List<ShippingAddress>();
			var identity = User.Identity;
			var currentUser = _userManager.Users.Single(u => u.UserName == identity.Name);
			var userId = currentUser.Id;

			var customer = await _context.Customers.Where(c => c.UserId == userId).SingleAsync();

			int shipTypeDefault = 1; //Type 1 = kleines Paket
			int countryDefault = 1;//Country 1 Deutschland
			int ShippingPeriodDefaultID = 1;

			if (customer.CustomerID != Guid.Empty)
			{
				countryDefault = customer.CountryId;
			}

			if (id != null)
			{
				Guid cartId = (Guid)id;
				var shoppingCart = await _context.ShoppingCarts.SingleAsync(sc => sc.ID == id);
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

					if (ShippingPeriodDefaultID < product.ShippingPeriod)
					{
						ShippingPeriodDefaultID = product.ShippingPeriod; 
					}

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
						PosPrice = Math.Round( pPrice, 2),
						Quantity = Math.Round(item.Quantity,2),
						ProductID = item.ProductID,
						Unit = unit,
						ProductName = product.Name,
						ProductNo = product.ProductNumber.ToString(),
						MinimumPurchaseQuantity = Math.Round(product.MinimumPurchaseQuantity,2),
						AvailableQuantity = Math.Round(product.AvailableQuantity,2),
						ShoppingCartID = shoppingCart.ID,
						SellBasePrice = Math.Round(item.SellBasePrice,2)
					};
					vmcLines.Add(cvml);
					total = total + pPrice;
				}

				CartViewModel cvm = new CartViewModel()
				{
					ID = shoppingCart.ID,
					Number = shoppingCart.Number,
					OrderId = shoppingCart.OrderId,
					Lines = vmcLines,
					Total = total
				};

				addresses = await _context.ShippingAddresses.Where(sh => sh.CustomerID == customer.CustomerID).ToListAsync();

				List<SelectItemViewModel> selectItemViewModels = new List<SelectItemViewModel>();
				foreach (var item in addresses)
				{
					SelectItemViewModel selectItemViewModel = new SelectItemViewModel { ID = item.ID, IsSelected = item.IsMainAddress, Name = item.Address + "\n" + item.PostCode + " " +  item.City};
					selectItemViewModels.Add(selectItemViewModel);
				}

				ViewData["Addresses"] = new SelectList( selectItemViewModels , "ID","Name");

				vm.Cart = cvm;
				
				vm.Order = new Order();
				vm.Order.Number = "AF-" + DateTime.Now.ToFileTimeUtc();
				vm.Order.CartID = cvm.ID;
				vm.Order.OrderDate = DateTime.Now;
			}

			var paymends = await _context.Paymends.ToListAsync();

			ShippingPrice defaultPrice = await _context.ShippingPrices.SingleAsync(s => s.ShippingPriceTypeId == shipTypeDefault && s.CountryId == countryDefault);

			ShippingPeriod periodDefault = await _context.ShpippingPeriods.SingleAsync(s => s.ShippingPeriodID == ShippingPeriodDefaultID);

			vm.Cart.Total = vm.Cart.Total + defaultPrice.Price;

			vm.Cart.Total = Math.Round(vm.Cart.Total, 2);

			vm.PayPalTotal = Math.Round(vm.Cart.Total, 2).ToString(CultureInfo.CreateSpecificCulture("en-US"));

			defaultPrice.Price = Math.Round(defaultPrice.Price, 2);

			vm.ShippingPrice = defaultPrice;

			vm.ShippingPeriod = periodDefault;

			ViewData["Paymends"] = paymends;


			return View(vm);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Number,OrderDate,PaymentID,IsPayed,IsSend,Shippingdate,IsClosed,CartID,ExceptLawConditions,ShippingAddressId,ShippingPriceId, ShippingPeriodId,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
				//TODO benutzer check und redirect anpassen!!!
				order.ID = Guid.NewGuid();
				order.OrderDate = DateTime.Now;
				if (User != null)
				{
					var userid = _userManager.GetUserId(User);
					Customer customer = await _context.Customers.SingleAsync(c => c.UserId == userid);
					order.CustomerID = customer.CustomerID;

					var shoppingCart = await _context.ShoppingCarts.SingleAsync(sc => sc.ID.Equals(order.CartID));

					var shoppingCartLines = await _context.ShoppingCartLines.Where(scl => scl.ShoppingCartID.Equals(shoppingCart.ID)).ToListAsync();

					var shipPrice = await _context.ShippingPrices.SingleAsync(p => p.ID == order.ShippingPriceId);

					order.ShippingPrice = shipPrice.Price;

					_context.Add(order);
					await _context.SaveChangesAsync();

					foreach (var item in shoppingCartLines)
					{
						var odl = new OrderLine { OrderID = order.ID, Position = item.Position, ProductID = item.ProductID, Quantity = item.Quantity, SellBasePrice = item.SellBasePrice, UnitID = item.UnitID };
						_context.Add(odl);
					}
					_logger.LogInformation($"Order Information:\n" +
							$"User:\t\t{User.Identity.Name},\n" +
							$"Order No:\t\t{order.Number},\n" +
							$"Order Date:\t\t{order.OrderDate}\n" +
							$"Ship to id:\t\t{order.ShippingAddressId}\n" +
						   $"Period id:\t\t{order.ShippingPeriodId}\n" +
						   $"Ship Price:\t\t{order.ShippingPrice}\n" +
						   $"Ship Price id:\t\t{order.ShippingPriceId}\n" +
						   $"Paymend id:\t\t{order.PaymentID}\n" +
						   $"customer id:\t\t{order.CustomerID}\n", null);

					await _context.SaveChangesAsync();

					_context.Entry(shoppingCart).State = EntityState.Deleted;
					await _context.SaveChangesAsync();

					HttpContext.Session.SetString("ShoppingCartId", string.Empty);

					string subject = "Ihre Bestellung bei marelibuDesign";
					string mailContent = await CreateOrderMail(order.ID);

					await _emailSender.SendEmailAsync(User.Identity.Name,subject, mailContent);
				}
				else
				{
					return NotFound();
				}
            }

			return RedirectToAction("WeHaveYourOrder",new { id = order.ID });
			//return View(order);
		}

		private async Task<IActionResult> RegisterGust(Guid? id)
		{
			return View();
		}

		private async Task<IActionResult> ShippingAddress(Guid? id)
		{
			var order = await _context.Orders.Where(o => o.ID.Equals(id)).SingleAsync();
			var customer = await _context.Customers.Where(c => c.CustomerID == order.CustomerID).SingleAsync();
			var vm = new ShippingAddressViewModel { Customer = customer, Guest = null, Order = order, ShippingAddress = new Models.ShippingAddress() };
			return View();
		}

		public async Task<IActionResult> Confirme(Guid? id)
		{
			var order = await _context.Orders.Where(o => o.ID.Equals(id)).SingleAsync();
			return View();
		}

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }

		private	async Task<string>CreateOrderMail(Guid? id)
		{
			string mail = string.Empty;

			if (id != null)
			{
				var order = await GetViewModel((Guid)id);

				string table = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" height=\"100 % \" width=\"100 % \" id=\"bodyTable\">" +
							"<tr>" +
								"<th align=\"center\">Position</th>" +
								"<th align=\"center\">Artikel Nr.</th>" +
								"<th align=\"center\">Artikel Name</th>" +
								"<th align=\"center\">Menge</th>" +
								"<th align=\"center\">Preis in &euro;</th>" +
							"</tr>";
				foreach(var item in order.OrderLines)
				{
					table += $"<tr>" +
								$"<td align=\"center\">{item.Position}</td>" +
								$"<td align=\"center\">{item.ProductNumber}</td>" +
								$"<td align=\"center\">{item.ProductName}</td>" +
								$"<td align=\"center\">{item.Quantity} {item.ProductUnit}</td>" +
								$"<td align=\"center\">{item.Price}</td>" +
							$"</tr>";
				}
				
				table += $"<tr>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\">Versand</td>" +
							$"<td align=\"center\">{order.ShipPriceName}</td>" +
							$"<td align=\"center\">1</td>" +
							$"<td align=\"center\">{order.ShipPrice}</td>" +
						$"</tr>";
				table += $"<tr>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\">{order.OrderShippingPeriod}</td>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\"></td>" +
						$"</tr>";

				table += $"<tr>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\"></td>" +
							$"<td align=\"center\"> Gesamt:</td>" +
							$"<td align=\"center\">{order.OrderTotal}</td>" +
						$"</tr>" +
					$"</table>";

				var shipTo = $"<h4>Lieferadresse</h4>" +
								$"<p>{order.OrderShippingAddress.FirstName} {order.OrderShippingAddress.LastName}</p>" +
								$"<p>{order.OrderShippingAddress.Address}</p>" +
								$"<p>{order.OrderShippingAddress.AdditionalAddress}</p>" +
								$"<p>{order.OrderShippingAddress.PostCode} {order.OrderShippingAddress.City}</p>" +
								$"<p>{order.CountryName}</p>";

				var tac = await _context.LawContents.SingleAsync(a => a.ID == 1);
				var cancel = await _context.LawContents.SingleAsync(c => c.ID == 2);

				string bank = string.Empty;

				if (order.Bank != null)
				{
					bank += $"<h4>Bankverbindung</h4>" +
								$"<p><strong>Kontoinhaber:</strong>{order.Bank.AccountOwner}</p>" +
								$"<p><strong>Bank:</strong>{order.Bank.Institute}</p>" +
								$"<p><strong>IBAN:</strong>{order.Bank.Iban}</p>" +
								$"<p><strong>Swift BIC:</strong>{order.Bank.SwiftBic}</p><br />";
				}

				mail = order.OrderThankYou;
				mail += "<hr />";
				mail += $"<strong>Auftragsnummer:</strong><p>{order.OrderNo}</p>";
				mail += $"<strong>Auftragsdatum:</strong><p>{order.OrderDate.Date}</p>";
				mail += $"<strong>Auftragsnummer:</strong><p>{order.OrderNo}</p>";
				mail += "<br />";
				mail += bank;
				mail += table;
				mail += "<br />";
				mail += shipTo;
				mail += "<br />";
				mail += "<hr />";
				mail += cancel.HtmlContent;
				mail += "<br />";
				mail += "<hr />";
				mail += tac.HtmlContent;

			}

			return mail;
		}

		private async Task<string> GetThankyou(Paymend paymend)
		{
			string thankyou = string.Empty;

			OrderCompletionText message;

			switch (paymend.PaymendType)
			{
				case Enums.PaymendTypeEnum.None:
					break;
				case Enums.PaymendTypeEnum.Prepay:
					message = await _context.OrderCompletionTexts.FirstAsync(c => c.PaymendType == Enums.PaymendTypeEnum.Prepay);
					thankyou = message.Content;
					break;
				case Enums.PaymendTypeEnum.PayPal:
					message = await _context.OrderCompletionTexts.FirstAsync(c => c.PaymendType == Enums.PaymendTypeEnum.PayPal);
					thankyou = message.Content;
					break;
				case Enums.PaymendTypeEnum.Bill:
					message = await _context.OrderCompletionTexts.FirstAsync(c => c.PaymendType == Enums.PaymendTypeEnum.Bill);
					thankyou = message.Content;
					break;
				default:
					break;
			}
			return thankyou;
		}

		private	async Task<WeHaveYourOrderViewModel>GetViewModel(Guid id)
		{
			var myorder = await _context.Orders.SingleAsync(o => o.ID == id);
			var paymend = await _context.Paymends.SingleAsync(p => p.ID == myorder.PaymentID);
			var shippingAddress = await _context.ShippingAddresses.SingleAsync(a => a.ID == myorder.ShippingAddressId);
			var country = await _context.Countries.SingleAsync(c => c.ID == shippingAddress.CountryID);
			var period = await _context.ShpippingPeriods.SingleAsync(p => p.ShippingPeriodID == myorder.ShippingPeriodId);
			var shipPrice = await _context.ShippingPrices.SingleAsync(p => p.ID == myorder.ShippingPriceId);

			var olines = await _context.OrderLines.Where(ol => ol.OrderID.Equals(myorder.ID)).ToListAsync();

			List<WeHaveYourOrderLineViewModel> lineViewModels = new List<WeHaveYourOrderLineViewModel>();

			foreach (var item in olines)
			{
				var prod = await _context.Products.SingleAsync(p => p.ProductID == item.ProductID);
				var img = await _context.ProductImages.SingleAsync(p => p.IsMainImage && p.ProductID == item.ProductID);
				var unit = await _context.Units.SingleAsync(u => u.UnitID == prod.BasesUnitID);
				var line = new WeHaveYourOrderLineViewModel
				{
					ID = item.OrderLineID,
					ImagePath = img.ImageUrl,
					Position = item.Position,
					Price = Math.Round((item.SellBasePrice * item.Quantity),2),
					ProductName = prod.Name,
					ProductNumber = prod.ProductNumber,
					Quantity = Math.Round(item.Quantity,2),
					ProductUnit = unit.Name
				};

				lineViewModels.Add(line);
			}


			BankAcccount bank = null;

			if (paymend.PaymendType == Enums.PaymendTypeEnum.Prepay)
			{
				bank = await _context.BankAcccounts.FirstAsync();
			}

			string thankyou = await GetThankyou(paymend);

			WeHaveYourOrderViewModel viewModel = new WeHaveYourOrderViewModel
			{
				OrderID = myorder.ID,
				OrderDate = myorder.OrderDate,
				OrderNo = myorder.Number,
				OrderPaymend = paymend.Name,
				OrderShippingAddress = shippingAddress,
				OrderShippingPeriod = period.Decription,
				Bank = bank,
				OrderTotal = Math.Round(myorder.Total,2),
				OrderThankYou = thankyou,
				ShipPrice = Math.Round(myorder.ShippingPrice,2),
				ShipPriceName = shipPrice.Name,
				CountryName = country.Name,
				OrderLines = lineViewModels
			};

			return viewModel;
		}

	}
}
