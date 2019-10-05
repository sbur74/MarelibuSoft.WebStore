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
using MarelibuSoft.WebStore.Common.Statics;

namespace MarelibuSoft.WebStore.Controllers
{
	[Authorize]
	public class OrdersController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger logger;
		private readonly ILoggerFactory factory;
		private readonly IEmailSender _emailSender;
		private ShoppingCartHelper cartHelper;
		private ShippingAddressHelper addressHelper;

		public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory, IEmailSender emailSender)
		{
			_context = context;
			_userManager = userManager;
			factory = loggerFactory;
			this.logger = factory.CreateLogger<OrdersController>();
			_emailSender = emailSender;
			cartHelper = new ShoppingCartHelper(_context, factory.CreateLogger<ShoppingCartHelper>());
			addressHelper = new ShippingAddressHelper(_context);
		}

		public IActionResult Checkout()
		{
			return View();
		}

		// GET: Orders
		public async Task<IActionResult> WeHaveYourOrder()
		{
			Guid id = Guid.Parse(HttpContext.Session.GetString("myOrder"));
			if (id == null)
			{
				return NotFound();
			}
			WeHaveYourOrderViewModel viewModel = await GetViewModel((Guid)id);
			viewModel.Email = User.Identity.Name;
			string subject = "Ihre Bestellung bei marelibuDesign";
			string mailContent = await CreateOrderMail(viewModel.OrderID);

			var agb = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.AGB);
			var wiederuf = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.WRB);
			var datenschutz = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.DSK);
			var attachments = new List<string> { agb.Filename, wiederuf.Filename, datenschutz.Filename };

			logger.LogDebug("WeHaveYourOrderViewModel -> try to send email",null);

			await _emailSender.SendEmailWithAttachmentsAsync(User.Identity.Name, subject, mailContent, attachments, "petra@marelibuDesign.de");
            await _emailSender.SendEmailWithAttachmentsAsync("pburon@t-online.de", subject, mailContent, attachments, "");
            await _emailSender.SendEmailAsync("petra@marelibuDesign.de", "Du hast etwas auf marelibudesign.de verkauft", $"<p>Verkauf an: {User.Identity.Name}</p>");

			//ViewData["Landing"] = viewModel;

			return View(viewModel);
		}
		// GET: Orders/Create
		public async Task<IActionResult> Create(Guid? id)
		{

			cartHelper.CheckAndRemove();

			OrderCreateViewModel vm = new OrderCreateViewModel();
			List<ShippingAddress> addresses = new List<ShippingAddress>();
			var identity = User.Identity;
			var currentUser = _userManager.Users.Single(u => u.UserName == identity.Name);
			var userId = currentUser.Id;

			var customer = await _context.Customers.Where(c => c.UserId == userId).SingleAsync();
			ShippingPrice shippingPriceDefault = null;
			decimal shipDefaultPrice = 0.0M;

			int countryDefault = 1;//Country 1 Deutschland
			int ShippingPeriodDefaultID = 1;

			if (customer.CustomerID != Guid.Empty)
			{
				var shipping = await _context.ShippingAddresses.SingleOrDefaultAsync(c => c.CustomerID == customer.CustomerID && c.IsMainAddress);

				if (shipping == null)
				{
					return RedirectToAction("CustomerIndex", "ShippingAddresses", new { id = customer.CustomerID });
				}

				countryDefault = shipping.CountryID;
			}

			var countries = await _context.Countries.ToListAsync();
			var country = countries.Single(c => c.ID == countryDefault);

			ViewData["CustomerCountryCode"] = country.Code;

			if (id != null)
			{
				Guid cartId = (Guid)id;
				var shoppingCart = await _context.ShoppingCarts
                                            .Include(l => l.Lines)
                                            .ThenInclude(t => t.ShoppingCartLineTextOptions)
                                            .Include(l => l.Lines)
                                            .ThenInclude(v => v.VariantValues)
                                            .SingleAsync(sc => sc.ID == id);
				decimal total = 0.0M;
                var lines = shoppingCart.Lines;
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
					var product = _context.Products
                                    .Include(v => v.ProductVariants)
                                    .ThenInclude(o => o.Options)
                                    .Where(p => p.ProductID.Equals(item.ProductID))
                                    .SingleOrDefault();

					if (ShippingPeriodDefaultID < product.ShippingPeriod)
					{
						ShippingPeriodDefaultID = product.ShippingPeriod;
					}
					var productShipPrice = await _context.ShippingPrices
                                                    .SingleAsync
                                                    (
                                                            s => s.ShippingPriceTypeId == product.ShippingPriceType
                                                            && s.CountryId == countryDefault
                                                     );

					if (shipDefaultPrice < productShipPrice.Price)
					{
						shipDefaultPrice = productShipPrice.Price;
						shippingPriceDefault = productShipPrice;
					}

                    decimal baseprice = Math.Round(item.SellBasePrice,2); // immer den an der Warenkorbzeile nehmen

                    decimal sekprice = Math.Round(product.SecondBasePrice, 2);

                    if (item.SellActionItemId > 0)
                    {
                        var sellActionItem = await _context.SellActionItems
                                                        .SingleOrDefaultAsync(i => i.SellActionItemID == item.SellActionItemId);
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
					var unitHelper = new UnitHelper(_context, factory);
					string unit = unitHelper.GetUnitName(product.BasesUnitID);
					string sekunit = unitHelper.GetUnitName(product.SecondBaseUnit);
                    var varinatList = new List<VariantViewModel>();
                    var textList = new List<TextOptionViewModel>();

                    foreach (ShoppingCartLineVariantValue variantValue in item.VariantValues)
                    {
                        string varinatname = product.ProductVariants.Single(v => v.ID == variantValue.ProductVariant).Name;
                        var clvvm = new VariantViewModel
                        {
                            Id = variantValue.Id,
                            Combi = variantValue.Combi,
                            Price = variantValue.Price,
                            ProductVariant = variantValue.ProductVariant,
                            ProductVariantOption = variantValue.ProductVariantOption,
                            Quantity = variantValue.Quantity,
                            LineId = variantValue.ShoppingCartLineId,
                            Value = variantValue.Value, 
                            VariantName = varinatname
                        };
                        varinatList.Add(clvvm);
                    }
                    foreach (ShoppingCartLineTextOption textOption in item.ShoppingCartLineTextOptions)
                    {
                        var cltxtvm = new TextOptionViewModel
                        {
                            ID = textOption.ID,
                            LineId = textOption.ShoppingCartLineId,
                            Text = textOption.Text
                        };
                        textList.Add(cltxtvm);
                    }

					CartLineViewModel cvml = new CartLineViewModel()
					{
						ID = item.ID,
						CartID = item.ShoppingCartID,
						ImgPath = path,
						Position = item.Position,
						PosPrice = Math.Round(pPrice, 2),
						Quantity = Math.Round(item.Quantity, 2),
						ProductID = item.ProductID,
						Unit = unit,
						ProductName = product.Name,
						ProductNo = product.ProductNumber.ToString(),
						MinimumPurchaseQuantity = Math.Round(product.MinimumPurchaseQuantity, 2),
						AvailableQuantity = Math.Round(product.AvailableQuantity, 2),
						ShoppingCartID = shoppingCart.ID,
						SellBasePrice = baseprice,
						SellSekPrice = sekprice,
                        SellActionItemId = item.SellActionItemId,
						SekUnit = sekunit,
						ShortDescription = product.ShortDescription,
						UnitID = product.BasesUnitID,
                        SlugUrl = $"{item.ProductID}-{product.ProductNumber}-{FriendlyUrlHelper.ReplaceUmlaute(product.Name)}",
                        VariantList = varinatList,
                        TextOptionList = textList
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
					Total = total,
				};

				addresses = await _context.ShippingAddresses.Where(sh => sh.CustomerID == customer.CustomerID).ToListAsync();

				vm.ShippingAddresseVMs = new List<ShippingAddressViewModel>();
				int mainShipID = 0;
				foreach (var item in addresses)
				{
					var shipVm = new ShippingAddressViewModel
					{
						ID = item.ID,
						FirstName = item.FirstName,
						LastName = item.LastName,
						Address = item.Address,
						AdditionalAddress = item.AdditionalAddress,
						PostCode = item.PostCode,
						City = item.City,
						CountryID = item.CountryID,
						CustomerID = item.CustomerID,
						IsMainAddress = item.IsMainAddress,
						CountryName = countries.Single(c => c.ID == item.CountryID).Name
					};
					if (shipVm.IsMainAddress) mainShipID = shipVm.ID;
					vm.ShippingAddresseVMs.Add(shipVm);
				}

				string strOrderNo = await GetActualOrderNo();

				vm.Cart = cvm;
				vm.Order = new Order();
				vm.Order.Number = strOrderNo;
				vm.Order.CartID = cvm.ID;
				vm.Order.ShippingAddressId = mainShipID;
				vm.Order.OrderDate = DateTime.Now;
			}

			var paymends = await _context.Paymends.ToListAsync();

			ShippingPeriod periodDefault = await _context.ShpippingPeriods.SingleAsync(s => s.ShippingPeriodID == ShippingPeriodDefaultID);

			vm.Cart.Total = vm.Cart.Total + shipDefaultPrice;

			vm.Cart.Total = Math.Round(vm.Cart.Total, 2);

			vm.PayPalTotal = Math.Round(vm.Cart.Total, 2).ToString(CultureInfo.CreateSpecificCulture("en-US"));

			shipDefaultPrice = Math.Round(shipDefaultPrice);

			vm.ShippingPrice = shippingPriceDefault;

			vm.ShippingPeriod = periodDefault;

			vm.CanBuyWithBill = customer.AllowedPayByBill;

			ViewData["Paymends"] = paymends;


			return View(vm);
		}

		private async Task<string> GetActualOrderNo()
		{
			int no = 0;
			try
			{
				var lastorder = await _context.Orders.OrderBy(o => o.OrderDate).LastAsync();

				if (lastorder != null)
				{
					string lastOrdNo = lastorder.Number;
					no = int.Parse(lastOrdNo.Substring(lastOrdNo.IndexOf("S") + 1));
					no++;
				}
				else
				{
					no = 1000;
				}
			}
			catch (Exception e)
			{
				logger.LogError(e, "OrderController.GetActualOrderNo -> Fehler beim ermitteln der Auftragsnummer");
			}
			string strOrderNo = $"{DateTime.Now.Year}{DateTime.Now.Month.ToString("d2")}{DateTime.Now.Day.ToString("d2")}S{no}";
			return strOrderNo;
		}

		// POST: Orders/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Number,OrderDate,PaymentID,IsPayed,IsSend,Shippingdate,IsClosed,CartID,ExceptLawConditions,ShippingAddressId,ShippingPriceId, ShippingPeriodId,Total,FreeText")] Order order)
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

					var shoppingCart = await _context.ShoppingCarts
                                                .Include(sl => sl.Lines)
                                                .ThenInclude(slt => slt.ShoppingCartLineTextOptions)
                                                .Include(sl => sl.Lines)
                                                .ThenInclude(slv => slv.VariantValues)
                                                .SingleAsync(sc => sc.ID.Equals(order.CartID));

					var shoppingCartLines = shoppingCart.Lines;

					var shipPrice = await _context.ShippingPrices.SingleAsync(p => p.ID == order.ShippingPriceId);

					order.ShippingPrice = shipPrice.Price;
					order.Number = await GetActualOrderNo(); //vor dem Speichern nochmal abrufen

					_context.Add(order);
					await _context.SaveChangesAsync();

					foreach (var item in shoppingCartLines)
					{
                        var product = await _context.Products
                                                .Include(p => p.ImageList)
                                                .Include(v => v.ProductVariants)
                                                .ThenInclude(o => o.Options)
                                                .SingleOrDefaultAsync(p => p.ProductID == item.ProductID);
                        int number = 0;
                        string name = string.Empty;
                        string imgurl = string.Empty;
                        if(product != null)
                        {
                            number = product.ProductNumber;
                            name = product.Name;
                            var url = product.ImageList.FirstOrDefault(i => i.IsMainImage);
                            if (url != null) { imgurl = url.ImageUrl; }
                        }
                        List<OrderLineTextOption> textOptions = new List<OrderLineTextOption>();
                        List<OrderLineVariantValue> orderLineVariants = new List<OrderLineVariantValue>();

                        if(item.ShoppingCartLineTextOptions.Count > 0)
                        {
                            foreach (var txtOption in item.ShoppingCartLineTextOptions)
                            {
                                var ordertxt = new OrderLineTextOption { 
                                    Text = txtOption.Text,
                                    Name = product.TextVariantTitel };
                                textOptions.Add(ordertxt);
                            }
                        }
                        if(item.VariantValues.Count > 0)
                        {
                            foreach (var variantvalue in item.VariantValues)
                            {
                                string vname = product.ProductVariants.Single(v => v.ID == variantvalue.ProductVariant).Name;
                                var ordervariant = new OrderLineVariantValue
                                {
                                    Combi = variantvalue.Combi,
                                    Price = variantvalue.Price,
                                    Quantity = variantvalue.Quantity,
                                    Value = variantvalue.Value,
                                    ProductVariant = variantvalue.ProductVariant,
                                    VarinatName = vname
                                };
                                orderLineVariants.Add(ordervariant);
                            }
                        }

						var odl = new OrderLine
                        {
                            OrderID = order.ID,
                            Position = item.Position,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            SellBasePrice = item.SellBasePrice,
                            UnitID = item.UnitID,
                            ProductName = name,
                            ProductNumber = number,
                            ImageUrl = imgurl,
                            OrderLineTextOptions = textOptions,
                            VariantValues = orderLineVariants
                        };
						_context.Add(odl);
					}
					logger.LogInformation($"Order Information:\n" +
							$"User:\t\t{User.Identity.Name},\n" +
							$"Order No:\t\t{order.Number},\n" +
							$"Order Date:\t\t{order.OrderDate}\n" +
							$"Ship to id:\t\t{order.ShippingAddressId}\n" +
						   $"Period id:\t\t{order.ShippingPeriodId}\n" +
						   $"Ship Price:\t\t{order.ShippingPrice}\n" +
						   $"Ship Price id:\t\t{order.ShippingPriceId}\n" +
						   $"Paymend id:\t\t{order.PaymentID}\n" +
						   $"FreeText:\t\t{order.FreeText}\n" +
						   $"customer id:\t\t{order.CustomerID}\n", null);

					await _context.SaveChangesAsync();

					_context.Entry(shoppingCart).State = EntityState.Deleted;
					await _context.SaveChangesAsync();

					HttpContext.Session.SetString("ShoppingCartId", string.Empty);

					if (order.PaymentID == 2)
					{
						return RedirectToAction("PayWithPayPal", new { id = order.ID });
					}
				}
				else
				{
					return NotFound();
				}
			}

			HttpContext.Session.SetString("myOrder", order.ID.ToString());
			return RedirectToAction("WeHaveYourOrder");
			//return RedirectToAction("Checkout", new { id = order.ID });
			//return View(order);
		}

		public async Task<IActionResult> PayWithPayPal(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var order = await _context.Orders.SingleAsync(o => o.ID.Equals(id));

			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PayWithPayPal([Bind("ID,Number,OrderDate,PaymentID,IsPayed,IsSend,Shippingdate,IsClosed,CartID,ExceptLawConditions,ShippingAddressId,ShippingPriceId, ShippingPeriodId,Total")] Order postorder)
		{
			if (postorder.ID == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.SingleAsync(o => o.ID.Equals(postorder.ID));

			order.IsPayed = postorder.IsPayed;
			_context.Entry(order).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return RedirectToAction("WeHaveYourOrder", new { id = order.ID });
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

		private async Task<string> CreateOrderMail(Guid? id)
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
				foreach (var item in order.OrderLines)
				{
                    bool hasVariants = item.TextOptionsList.Count > 0 || item.VariantList.Count > 0;
                    if (!hasVariants)
                    {
                        table += $"<tr>" +
                                                $"<td align=\"center\">{item.Position}</td>" +
                                                $"<td align=\"center\">{item.ProductNumber}</td>" +
                                                $"<td align=\"center\">{item.ProductName}</td>" +
                                                $"<td align=\"center\">{item.Quantity} {item.ProductUnit}</td>" +
                                                $"<td align=\"center\">{item.Price}</td>" +
                                            $"</tr>"; 
                    }
                    else
                    {
                        table += $"<tr>" +
                                                $"<td align=\"center\">{item.Position}</td>" +
                                                $"<td align=\"center\">{item.ProductNumber}</td>" +
                                                $"<td align=\"center\">{item.ProductName}</td>" +
                                                $"<td align=\"center\">{item.Quantity} {item.ProductUnit}</td>" +
                                                $"<td align=\"center\">{item.Price}</td>" +
                                            $"</tr>";
                        foreach (var txt in item.TextOptionsList)
                        {
                            table += $"<tr>" +
                                        $"<td align=\"center\">Text:</td>"+
                                        $"<td align=\"center\" colspan=\"4\">{txt.Text}</td>" +
                                $"</tr>";
                        }
                        foreach (var variant in item.VariantList)
                        {
                            table += $"<tr>" +
                                         $"<td align=\"center\">{variant.VariantName}</td>" +
                                         $"<td align=\"center\" colspan=\"4\">{variant.Value}</td>" +
                                 $"</tr>";
                        }
                    }
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
								$"<p>{order.OrderShippingAddress.CompanyName}</p>" +
								$"<p>{order.OrderShippingAddress.Address}</p>" +
								$"<p>{order.OrderShippingAddress.AdditionalAddress}</p>" +
								$"<p>{order.OrderShippingAddress.PostCode} {order.OrderShippingAddress.City}</p>" +
								$"<p>{order.OrderShippingAddress.CountryName}</p>";

				var invoice = $"<h4>Rechnungsadresse</h4>" +
							$"<p>{order.OrderInvoiceAddress.FirstName} {order.OrderInvoiceAddress.LastName}</p>" +
							$"<p>{order.OrderInvoiceAddress.CompanyName}</p>" +
							$"<p>{order.OrderInvoiceAddress.Address}</p>" +
							$"<p>{order.OrderInvoiceAddress.PostCode} {order.OrderInvoiceAddress.City}</p>" +
							$"<p>{order.OrderInvoiceAddress.CountryName}</p>";


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
				mail += table;
				mail += "<br />";
				mail += invoice;
				mail += "<br />";
				mail += shipTo;
				mail += "<br />";
				mail += $"<p>Vielen Dank für Ihren Einkauf.</p>";
				mail += $"<p>Viele Gr&uuml;&szlig;e<br /> Petra Buron<br /><a href=\"www.marelibuDesign.de\">www.marelibuDesign.de</a>";
			}

			return mail;
		}

		private async Task<string> GetThankyou(Paymend paymend)
		{
			string thankyou = string.Empty;

			OrderCompletionText message;

			try
			{
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
			}
			catch (Exception e)
			{
				logger.LogError(e, "OrdersController.GetThankyou -> Fehler bei abruf der Auftragsbesätigung");
			}

			if (string.IsNullOrWhiteSpace(thankyou))
			{
				thankyou = "Danke f&uuml;r Ihren Einkauf bei marelibuDesign";
			}

			return thankyou;
		}

		private async Task<WeHaveYourOrderViewModel> GetViewModel(Guid id)
		{
			var myorder = await _context.Orders.SingleAsync(o => o.ID == id);
			var paymend = await _context.Paymends.SingleAsync(p => p.ID == myorder.PaymentID);
			var shippingAddress = await _context.ShippingAddresses.SingleAsync(a => a.ID == myorder.ShippingAddressId);
			var invioceAddress = await _context.ShippingAddresses.FirstOrDefaultAsync(a => a.CustomerID == myorder.CustomerID && a.IsInvoiceAddress);
			
			var period = await _context.ShpippingPeriods.SingleAsync(p => p.ShippingPeriodID == myorder.ShippingPeriodId);
			var shipPrice = await _context.ShippingPrices.SingleAsync(p => p.ID == myorder.ShippingPriceId);

			var olines = await _context.OrderLines
                                .Include(ot => ot.OrderLineTextOptions)
                                .Include(ov => ov.VariantValues)
                                .Where(ol => ol.OrderID.Equals(myorder.ID)).ToListAsync();

			List<WeHaveYourOrderLineViewModel> lineViewModels = new List<WeHaveYourOrderLineViewModel>();

			foreach (var item in olines)
			{
				var prod = await _context.Products
                                         .Include(v => v.ProductVariants)
                                         .ThenInclude(o => o.Options)
                                         .SingleAsync(p => p.ProductID == item.ProductID);
				var img = await _context.ProductImages.SingleAsync(p => p.IsMainImage && p.ProductID == item.ProductID);
				var unit = await _context.Units.SingleAsync(u => u.UnitID == prod.BasesUnitID);
                var variantValues = new List<VariantViewModel>();
                var textOptions = new List<TextOptionViewModel>();
                if(item.VariantValues.Count > 0)
                {
                    foreach (var variant in item.VariantValues)
                    {
                        var vvvm = new VariantViewModel
                        {
                            Combi = variant.Combi,
                            Id = variant.Id,
                            Price = variant.Price,
                            Quantity = variant.Quantity,
                            ProductVariant = variant.ProductVariant,
                            ProductVariantOption = variant.ProductVariantOption,
                            Value = variant.Value, 
                            VariantName = variant.VarinatName
                        };
                        variantValues.Add(vvvm);
                    }
                }
                if (item.OrderLineTextOptions.Count > 0)
                {
                    foreach (var txt in item.OrderLineTextOptions)
                    {
                        var otvm = new TextOptionViewModel
                        {
                            ID = txt.ID,
                            LineId = txt.OrderLineId,
                            Text = txt.Text
                        };
                        textOptions.Add(otvm);
                    }
                }
				var line = new WeHaveYourOrderLineViewModel
				{
					ID = item.OrderLineID,
					ImagePath = img.ImageUrl,
					Position = item.Position,
					Price = Math.Round((item.SellBasePrice * item.Quantity), 2),
					ProductName = prod.Name,
					ProductNumber = prod.ProductNumber,
					Quantity = Math.Round(item.Quantity, 2),
					ProductUnit = unit.Name,
                    VariantList = variantValues,
                    TextOptionsList = textOptions
				};

				lineViewModels.Add(line);
			}


			BankAcccount bank = null;

			var shipTo = addressHelper.GetViewModel(shippingAddress);

			ShippingAddressViewModel invoiceVm = null;
		
			if(invioceAddress == null)
			{
				var customer = await _context.Customers.SingleAsync(c => c.CustomerID == myorder.CustomerID);
				invoiceVm = addressHelper.GetViewModel(customer);
			}
			else
			{
				invoiceVm = addressHelper.GetViewModel(invioceAddress);
			}

			string thankyou = await GetThankyou(paymend);

			WeHaveYourOrderViewModel viewModel = new WeHaveYourOrderViewModel
			{
				OrderID = myorder.ID,
				OrderDate = myorder.OrderDate,
				OrderNo = myorder.Number,
				OrderPaymend = paymend.Name,
				OrderShippingAddress = shipTo,
				OrderInvoiceAddress = invoiceVm,
				OrderShippingPeriod = period.Decription,
				Bank = bank,
				OrderTotal = Math.Round(myorder.Total, 2),
				OrderThankYou = thankyou,
				ShipPrice = Math.Round(myorder.ShippingPrice, 2),
				ShipPriceName = shipPrice.Name,
				OrderLines = lineViewModels,
				FreeText = myorder.FreeText
			};

			return viewModel;
		}

	}
}