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
using Microsoft.AspNetCore.Authorization;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using Microsoft.Extensions.Logging;
using MarelibuSoft.WebStore.Services;
using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Hosting;
using MarelibuSoft.WebStore.Common.Statics;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;
		private readonly IEmailSender _emailSender;
		private IHostingEnvironment _environment;
		private ShippingAddressHelper addressHelper;

		public OrdersController(ApplicationDbContext context,ILogger<OrdersController>logger, IEmailSender emailSender, IHostingEnvironment environment)
        {
            _context = context;
			_logger = logger;
			_emailSender = emailSender;
			_environment = environment;
			addressHelper = new ShippingAddressHelper(_context);
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
			List<OrderViewModel> vms = new List<OrderViewModel>();
			var orders = await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();
			foreach (var order in orders)
			{
				var vm = await GetOrderViewModel(order.ID);
				vms.Add(vm);
			}
			return View(vms);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .SingleOrDefaultAsync(m => m.ID == id);
		
            if (order == null)
            {
                return NotFound();
            }

			var vm = await GetOrderViewModel(order.ID);

			vm.StateViewModel = new OrderStateViewModel() { ID = order.ID };

			return View(vm);
        }

		public async Task<IActionResult> EMailSend(Guid? id)
		{
			var order = await _context.Orders.SingleAsync(o => o.ID.Equals(id));
			var customer = await _context.Customers.SingleAsync(c => c.CustomerID.Equals(order.CustomerID));
			var user = await _context.Users.SingleAsync(u => u.Id.Equals(customer.UserId));
			OrderEmailViewModel vm = new OrderEmailViewModel {
				OrderID = order.ID, Subject = $"Auftrags-Nr.: {order.Number}, am {order.OrderDate.Day}.{order.OrderDate.Month}.{order.OrderDate.Year}", Email = user.Email, Message = "<h3>Rechnung</h3>"
			};
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EMailSend([Bind("OrderID, Email, Subject, Message, Attachments")] OrderEmailViewModel orderEmail)
		{
			if (ModelState.IsValid)
			{
				var files = HttpContext.Request.Form.Files;
				if (files != null && files.Count > 0)
				{
					//var file = files.First();
					var helper = new UploadHelper(_environment);
					var todel = new List<string>();
					var attachments = new List<string>();

					foreach (var file in files)
					{
						var fnames = await helper.FileUploadAsync(file, "files", false);
						todel.Add(fnames.Filename);
						attachments.Add(fnames.Filename);
					}
					

					var agb = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.AGB);
					var wiederruf = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.WRB);
					var datenschutz = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.DSK);

					attachments.Add(agb.Filename);
					attachments.Add(wiederruf.Filename);
					attachments.Add(datenschutz.Filename);

					
					await _emailSender.SendEmailWithAttachmentsAsync(orderEmail.Email, orderEmail.Subject, orderEmail.Message, attachments);
					foreach (string file in todel)
					{
						helper.DeleteFile("files", file);
					}
				}
				
				return RedirectToAction(nameof(Index));
			}
			return View(orderEmail);
		}

		// GET: Admin/Orders/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
			var vm = await GetOrderViewModel(order.ID);

			return View(vm);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Number,OrderDate,PaymentID,IsPayed,IsSend,Shippingdate,IsClosed")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
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
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

			var vm = await GetOrderViewModel(order.ID);

			return View(vm);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
			//TODO if not send not payed dann verfügbare menge Product anpassen
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		[HttpPost, ActionName("SendEmail")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendEmailAsync(Guid? id, [Bind("ID", "StateAction","Message")] OrderStateViewModel viewModel)
		{
			if (id == null)
			{
				return NotFound();
			}

			_logger.LogDebug($"Admin/Orders/SendEmail values: id = {id}, action = {viewModel.StateAction}, message = {viewModel.Message}");

			Guid orderid =(Guid) id;

			Order order = await _context.Orders.SingleAsync(o => o.ID.Equals(orderid));
			order.IsSend = true;
			order.Shippingdate = DateTime.Now;

			_context.Entry(order).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			if(order == null)
			{
				return NotFound();
			}

			var ordervm = await GetOrderViewModel(orderid);

			string subject = "Versand von Auftrag " + ordervm.Number;
			string email = ordervm.EMail;
			string message = $"<p>Hallo {ordervm.CustomerFirstName},</p>" +
				$"<p>ich habe die Ware zu deinem Auftrag {ordervm.Number} vom {ordervm.OrderDate.ToShortDateString()} am {DateTime.Now.ToShortDateString()} versendet:</p>" +
				$"<table><tr><th>Position</th><th>Artikel-Nr.</th><th>Artikelname</th><th>Menge</th></tr>";
			foreach (var item in ordervm.OderLines)
			{
				message += $"<tr><td>{item.Position}</td><td>{item.ProductNumber}</td><td>{item.ProductName}</td><td>{Math.Round(item.OrderQuantity,2)} {item.OrderUnit}</td></tr>";
			}
			message += $"</table>";
			message += $"<p><strong>Sendungsinformationen:</strong></p>";
			message += $"<p>{viewModel.Message}</p>";
			message += $"<p>Vielen Dank für deinen Einkauf.</p>";
			message += $"<p>Viele Gr&uuml;&szlig;e<br /> Petra Buron<br />";

			var agb = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.AGB);
			var wiederuf = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.WRB);
			var datenschutz = await _context.ShopFiles.SingleAsync(s => s.ShopFileType == Enums.ShopFileTypeEnum.DSK);
			var attachments = new List<string> { agb.Filename, wiederuf.Filename, datenschutz.Filename };
			await _emailSender.SendEmailWithAttachmentsAsync(email,  subject, message, attachments);
			return RedirectToAction(nameof(Details), new { id = id });
		}

		[HttpPost, ActionName("MarkPayed")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> MarkPayedAsync(Guid? id, [Bind("ID", "StateAction", "Message")] OrderStateViewModel viewModel)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.SingleAsync(o => o.ID == id);
			if (order == null)
			{
				return NotFound();
			}

			order.IsPayed = true;
			_context.Entry(order).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Details), new { id = id });
		}
		[HttpPost, ActionName("MarkClosed")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> MarkClosedAsync(Guid? id, [Bind("ID", "StateAction", "Message")] OrderStateViewModel viewModel)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.SingleAsync(o => o.ID == id);
			if (order == null)
			{
				return NotFound();
			}

			order.IsClosed = true;
			_context.Entry(order).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Details), new { id = id });
		}


		private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }

		private async Task<OrderViewModel> GetOrderViewModel(Guid orderId)
		{
			OrderViewModel vm = null;
			try
			{
				var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == orderId);
				var olines = await _context.OrderLines.Where(ol => ol.OrderID.Equals(order.ID)).ToListAsync();
				List<OrderLineViewModel> lineViewModels = new List<OrderLineViewModel>();
				foreach (var item in olines)
				{
					Product product = null;

					product = await _context.Products.SingleAsync(p => p.ProductID == item.ProductID);
					var mainImage = await _context.ProductImages.SingleAsync(i => i.IsMainImage && i.ProductID.Equals(item.ProductID));
					var unit = await _context.Units.SingleAsync(u => u.UnitID == item.UnitID);

					OrderLineViewModel lineViewModel = new OrderLineViewModel {
						ID = item.OrderLineID,
						Image = mainImage.ImageUrl,
						OrderPrice = item.SellBasePrice,
						OrderQuantity = item.Quantity,
						ProductName = product.Name,
						OrderUnit = unit.Name,
						OrderLineTotal = 
						item.Quantity * item.SellBasePrice,
						Position = item.Position,
						ProductNumber = product.ProductNumber
					};

					lineViewModels.Add(lineViewModel);
				}

				
				var shippingPrice = await _context.ShippingPrices.SingleAsync(sp => sp.ID == order.ShippingPriceId);
				var shippingPeriod = await _context.ShpippingPeriods.SingleAsync(p => p.ShippingPeriodID == order.ShippingPeriodId);
				var paymend = await _context.Paymends.SingleAsync(p => p.ID == order.PaymentID);
				var customer = await _context.Customers.SingleAsync(c => c.CustomerID.Equals(order.CustomerID));
				var user = await _context.Users.SingleAsync(u => u.Id.Equals(customer.UserId));

				var shippingAddress = await _context.ShippingAddresses.FirstOrDefaultAsync(s => s.ID == order.ShippingAddressId);

				var invoiceaddress = await _context.ShippingAddresses.FirstOrDefaultAsync(c => c.CustomerID == order.CustomerID && c.IsInvoiceAddress);

				ShippingAddressViewModel shipToAddress = null;

				if (shippingAddress != null)
				{
					shipToAddress = addressHelper.GetViewModel(shippingAddress);
				}

				ShippingAddressViewModel invoiceVm = null;

				if (invoiceaddress == null)
				{
					invoiceVm = addressHelper.GetViewModel(customer);
				}
				else
				{
					invoiceVm = addressHelper.GetViewModel(invoiceaddress);
				}

				vm = new OrderViewModel {
					ID = order.ID,
					EMail = user.UserName,
					ExceptLawConditions = order.ExceptLawConditions,
					IsClosed = order.IsClosed,
					IsPayed = order.IsPayed,
					IsSend = order.IsSend,
					Number = order.Number,
					OrderDate = order.OrderDate,
					FreeText = order.FreeText,
					Payment = paymend.Name,
					Shippingdate = order.Shippingdate,
					ShippingPriceAtOrder = order.ShippingPrice,
					ShippingPriceName = shippingPrice.Name,
					ShippToAddress = shipToAddress,
					InvoiceAddress = invoiceVm,
					ShippingPeriodString = shippingPeriod.Decription,
					Total = order.Total,
					CustomerFirstName = customer.FirstName,
					CutomerLastName = customer.Name,
					OderLines = lineViewModels
				};
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Admin.OrdersController.GetViewModel -> error on sql transaction", null);
			}

			return vm;
		}
    }
}

