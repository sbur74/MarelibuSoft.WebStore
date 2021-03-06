﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.Extensions.Logging;
using MarelibuSoft.WebStore.Models.ViewModels;

namespace MarelibuSoft.WebStore.Controllers
{
    public class CustomerOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILoggerFactory factory;
		private readonly ILogger logger;

        public CustomerOrdersController(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
			factory = loggerFactory;
			logger = factory.CreateLogger<CustomerOrdersController>();
        }

        // GET: CustomerOrders
        public async Task<IActionResult> Index(Guid? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var orders = await _context.Orders.Where(o => o.CustomerID.Equals(id)).ToListAsync();

			var myorders = new List<CustomerIndexOrderViewModel>();

			foreach (var item in orders)
			{
				string state = "";
				if (!item.IsSend && !item.IsPayed && !item.IsClosed) state = "In Bearbeitung";
				if (item.IsPayed) state = "Bezahlt";
				if (item.IsSend) state = "Versendet";
				if (item.IsSend && !item.IsPayed) state = "Versendet, noch nicht bezahlt";
				if (item.IsClosed) state = "Abgeschlossen";
				var myorder = new CustomerIndexOrderViewModel
				{
					ID = item.ID,
					Number = item.Number,
					Orderdate = item.OrderDate.ToShortDateString(),
					OrderState = state
				};
				myorders.Add(myorder);
			}

			return View(myorders);
        }

        // GET: CustomerOrders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
			var olines = await _context.OrderLines.Where(o => o.OrderID.Equals(order.ID)).ToListAsync();
			var shipTo = await _context.ShippingAddresses.SingleAsync(s => s.ID == order.ShippingAddressId);
			var invoice = await _context.ShippingAddresses.SingleAsync(i => i.CustomerID == order.CustomerID && i.IsInvoiceAddress);
			var period = await _context.ShpippingPeriods.SingleAsync(p => p.ShippingPeriodID == order.ShippingPeriodId);
			var country = await _context.Countries.SingleAsync(c => c.ID == shipTo.CountryID);
			var incountry = await _context.Countries.SingleAsync(c => c.ID == invoice.CountryID);
			var payment = await _context.Paymends.SingleAsync(p => p.ID == order.PaymentID);

			var units = await _context.Units.ToListAsync();

			var custOrderLines = new List<CustomerOrderLineViewModel>();
			foreach (OrderLine orderLine in olines)
			{
				var product = await _context.Products.SingleAsync(p => p.ProductID == orderLine.ProductID);
				var custOLine = new CustomerOrderLineViewModel
				{
					OrderID = order.ID,
					OrderLineID = orderLine.OrderLineID,
					Position = orderLine.Position,
					ProductID = orderLine.ProductID,
					ProductName = product.Name,
					ProductNumber = product.ProductNumber.ToString(),
					Quantity = orderLine.Quantity,
					SellBasePrice = orderLine.SellBasePrice,
					Unit = units.Single(s => s.UnitID == orderLine.UnitID).Name
				};
				custOrderLines.Add(custOLine);
			}

			string addressStr = shipTo.Address;
			if (!string.IsNullOrWhiteSpace(shipTo.AdditionalAddress))
			{
				addressStr += "," + shipTo.AdditionalAddress;
			}

			string shiptoname = $"{shipTo.LastName}, {shipTo.FirstName}";
			if (!string.IsNullOrWhiteSpace(shipTo.CompanyName)) shiptoname += $", {shipTo.CompanyName}";
			string invoicename = $"{invoice.FirstName}, {invoice.LastName}";
			if (!string.IsNullOrWhiteSpace(invoice.CompanyName)) invoicename += $", {invoice.CompanyName}";
			string invoiceaddr = $"{invoice.Address}";
			if (!string.IsNullOrWhiteSpace(invoice.AdditionalAddress)) invoiceaddr += $", {invoice.AdditionalAddress}";
			

			var myorder = new CustomerOrderViewModel {
				ID = order.ID,
				ShippingCountryName = country.Name,
				CustomerID = order.CustomerID,
				ExceptLawConditions = order.ExceptLawConditions,
				FreeText = order.FreeText,
				IsClosed = order.IsClosed,
				IsPayed = order.IsPayed,
				IsSend = order.IsSend,
				Lines = custOrderLines,
				Number = order.Number,
				OrderDate = order.OrderDate,
				PaymentName = payment.Name,
				ShippingAddressName = shiptoname,
				ShippingAddressString = addressStr,
				Shippingdate = order.Shippingdate,
				ShipPeriod = period.Decription,
				ShippingPostCodeCity = $"{shipTo.PostCode}, {shipTo.City}", 
				ShipPrice = order.ShippingPrice,
				InvoiceAddressName = invoicename,
				InvoiceAddressString = invoiceaddr,
				InvoicePostCodeCity = $"{invoice.PostCode} {invoice.City}",
				InvoiceCountryName = incountry.Name,
				Total = order.Total 
			};

            return View(myorder);
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
