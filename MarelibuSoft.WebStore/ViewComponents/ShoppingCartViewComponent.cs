using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MarelibuSoft.WebStore.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
		private ApplicationDbContext _context;

		public ShoppingCartViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			Guid cartID;
			string sessionCartId = HttpContext.Session.GetString("ShoppingCartId");

			if (!string.IsNullOrEmpty(sessionCartId))
			{
				cartID = Guid.Parse(sessionCartId);
			}
			else
			{
				cartID = Guid.NewGuid();
				HttpContext.Session.SetString("ShoppingCartId", cartID.ToString());
			}

			ShoppingCart cart = await _context.ShoppingCarts.Where(c => c.ID.Equals(cartID)).FirstOrDefaultAsync();

			if (cart == null)
			{
				cart = new ShoppingCart() { ID = cartID, Number = "MD" + DateTime.Now.ToFileTimeUtc() };

				_context.ShoppingCarts.Add(cart);
				_context.SaveChanges();
			}
			else
			{
				cart = await _context.ShoppingCarts.Where(c => c.ID.Equals(cartID)).Include(l => l.Lines).FirstOrDefaultAsync();
			}

			if (cart.Lines == null)
			{
				cart.Lines = new List<ShoppingCartLine>();
			}

			return View(cart);
		}

	}
}
