using MarelibuSoft.WebStore.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Models;

namespace MarelibuSoft.WebStore.ViewComponents
{
	public class NavigationViewComponent : ViewComponent
	{
		private ApplicationDbContext _context;

		public NavigationViewComponent(ApplicationDbContext context)
		{
			_context = context;				
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<NavItemViewModel> navItems = new List<NavItemViewModel>();
			var catagories = await _context.Categories.ToListAsync();

			foreach (Category category in catagories)
			{
				
				var subs = _context.CategorySubs.Where(s => s.CategoryID == category.ID).ToList();
				var nav = new NavItemViewModel { Id = category.ID, Name = category.Name, PerentId = 0 };

				if (subs != null)
				{
					nav.Childs = new List<NavItemViewModel>();
					foreach (CategorySub categorysub in subs)
					{
						var details = _context.CategoryDetails.Where(d => d.CategorySubID == categorysub.ID).ToList();
						var navsub = new NavItemViewModel { Id = categorysub.ID, Name = categorysub.Name, PerentId = categorysub.CategoryID };

						if (details != null)
						{
							navsub.Childs = new List<NavItemViewModel>();
							foreach (CategoryDetail categorydetail in details)
							{
								var navdetail = new NavItemViewModel { Id = categorydetail.ID, Name = categorydetail.Name, PerentId = categorydetail.CategorySubID, Childs = null };
								navsub.Childs.Add(navdetail);
							}
						}

						nav.Childs.Add(navsub);
					}
				}

				navItems.Add(nav);
			}

			return View(navItems);
		}
	}
}
