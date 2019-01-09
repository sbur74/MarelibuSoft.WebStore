using MarelibuSoft.WebStore.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Models;
using Microsoft.Extensions.Logging;
using MarelibuSoft.WebStore.Common.Statics;

namespace MarelibuSoft.WebStore.ViewComponents
{
	public class NavigationViewComponent : ViewComponent
	{
		private ApplicationDbContext _context;
		private readonly ILogger _logger;

		public NavigationViewComponent(ApplicationDbContext context, ILogger<NavigationViewComponent> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<NavItemViewModel> navItems = new List<NavItemViewModel>();

			try
			{
				var catagories = await _context.Categories.ToListAsync();
				foreach (Category category in catagories)
				{

					var subs = _context.CategorySubs.Where(s => s.CategoryID == category.ID).ToList();
					var nav = new NavItemViewModel { Id = category.ID, Name = category.Name, PerentId = 0 , SlugUrl = $"{category.ID}-{FriendlyUrlHelper.ReplaceUmlaute(category.Name)}" };

					if (subs != null)
					{
						nav.Childs = new List<NavItemViewModel>();
						foreach (CategorySub categorysub in subs)
						{
							var details = _context.CategoryDetails.Where(d => d.CategorySubID == categorysub.ID).ToList();
							var navsub = new NavItemViewModel { Id = categorysub.ID, Name = categorysub.Name, PerentId = categorysub.CategoryID, SlugUrl = $"{categorysub.ID}-{FriendlyUrlHelper.ReplaceUmlaute(categorysub.Name)}" };

							if (details != null)
							{
								navsub.Childs = new List<NavItemViewModel>();
								foreach (CategoryDetail categorydetail in details)
								{
									var navdetail = new NavItemViewModel { Id = categorydetail.ID, Name = categorydetail.Name, PerentId = categorydetail.CategorySubID, Childs = null, SlugUrl = $"{categorydetail.ID}-{FriendlyUrlHelper.ReplaceUmlaute(categorydetail.Name)}" };

									_logger.LogDebug($"NavigationViewComponent.InvokeAsync -> add detail category: {navdetail.Name}");
									navsub.Childs.Add(navdetail);
								}
							}

							_logger.LogDebug($"NavigationViewComponent.InvokeAsync -> add sub category: {navsub.Name}");
							nav.Childs.Add(navsub);
						}
					}
					_logger.LogDebug($"NavigationViewComponent.InvokeAsync -> add category: {nav.Name}");
					navItems.Add(nav);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error on Invoke navigation!!!", null);
			}

			return View(navItems);
		}
	}
}
