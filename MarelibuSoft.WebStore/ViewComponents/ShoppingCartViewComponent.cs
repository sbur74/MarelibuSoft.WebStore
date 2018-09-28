using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
		private ApplicationDbContext _context;
		private readonly ILogger _logger;

		public ShoppingCartViewComponent(ApplicationDbContext context, ILogger<ShoppingCartViewComponent>logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			Guid cartID;
			string sessionCartId = HttpContext.Session.GetString("ShoppingCartId");
			_logger.LogDebug($"ShoppingCartViewComponent.InvokeAsync ->session: {HttpContext.Session.Id}, cart id from http context: {sessionCartId}");
			ShoppingCart cart = null;

			if (!string.IsNullOrEmpty(sessionCartId))
			{
				cartID = Guid.Parse(sessionCartId);
				cart = await _context.ShoppingCarts.Where(c => c.ID.Equals(cartID)).Include(l => l.Lines).FirstOrDefaultAsync();

				if (!cart.SessionId.Equals(HttpContext.Session.Id))
				{
					cart.SessionId = HttpContext.Session.Id;
				}

				cart.LastChange = DateTime.Now;
				_context.Entry(cart).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				_logger.LogDebug($"ShoppingCartViewComponent.InvokeAsync ->session: {HttpContext.Session.Id}, cart id is fill from http context: {sessionCartId}");
			}
			else
			{
				cartID = Guid.NewGuid();
				HttpContext.Session.SetString("ShoppingCartId", cartID.ToString());
				_logger.LogDebug($"ShoppingCartViewComponent.InvokeAsync -> creat new cart, id: {cartID}");
				cart = new ShoppingCart() {
					ID = cartID,
					Number = "MD" + DateTime.Now.ToFileTimeUtc(),
					CreateAt = DateTime.Now,
					LastChange = DateTime.Now,
					TabCounter = 1,
					SessionId = HttpContext.Session.Id
				};
				_context.ShoppingCarts.Add(cart);
				_context.SaveChanges();
			}

			if (cart.Lines == null)
			{
				_logger.LogDebug($"ShoppingCartViewComponent.InvokeAsync -> no lines in cart");
				cart.Lines = new List<ShoppingCartLine>();
			}

			return View(cart);
		}

	}
}
