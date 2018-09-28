using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Administrator, PowerUser")]
	public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
			var vm = new AdminHomeViewModel(); 

			var openOrders = _context.Orders.Where(o => o.IsClosed == false);
			var notpayedOrders = _context.Orders.Where(o => o.IsClosed == false && o.IsPayed == false );
			var sendOrders = _context.Orders.Where(o => o.IsClosed == false && o.IsSend == true);
			var closedOrders = _context.Orders.Where(o => o.IsClosed == true);

			vm.OpenOrdersCount = openOrders.Count();
			vm.OpenNotPayedOrdersCount = notpayedOrders.Count();
			vm.OpenSendOrdersCount = sendOrders.Count();
			vm.ClosedOrdersCount = closedOrders.Count();


			return View(vm);
        }
    }
}