﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models.ViewModels;
using Microsoft.Extensions.Logging;
using MarelibuSoft.WebStore.Services;
using MarelibuSoft.WebStore.Common.Helpers;
using MarelibuSoft.WebStore.Common.Statics;

namespace MarelibuSoft.WebStore.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly ILoggerFactory factory;
		private readonly ILogger _logger;
		private readonly IMetaService metaService;
		private ShoppingCartHelper cartHelper;

		public HomeController(ApplicationDbContext context, ILoggerFactory loggerFactory, IMetaService service)
		{
			_context = context;
			factory = loggerFactory;
			_logger = factory.CreateLogger<HomeController>();
			metaService = service;
			cartHelper = new ShoppingCartHelper(_context, factory.CreateLogger<ShoppingCartHelper>());
		}

		public IActionResult Index()
        {
			var model = new HomeViewModel();
			var productImages = _context.ProductImages.Where(i => i.IsMainImage).ToList();
			var urls = new List<SliderViewModel>();
			var startpage = _context.CmsStartPages.LastOrDefault();

			metaService.AddMetadata("description", "Verkauf von Eislaufasseoirs, Stoffen und mehr.");
			metaService.AddMetadata("keywords", "Asseoirs,Eiskunstlauf,Stoffe,Webrare,Taschen");

			cartHelper.CheckAndRemove();

			_logger.LogDebug("show home index");

			if (startpage != null)
			{
				if (!string.IsNullOrWhiteSpace(startpage.SeoDescription))
				{
					metaService.AddMetadata("description", startpage.SeoDescription);
				}
				if (!string.IsNullOrWhiteSpace(startpage.SeoKeywords))
				{
					metaService.AddMetadata("keywords", "Asseoirs,Eiskunstlauf,Stoffe,Webrare,Taschen");
				}
			}

			foreach (var item in productImages)
			{
				var artikel = _context.Products.Single(p => p.ProductID == item.ProductID);
                var sellActionItem = _context.SellActionItems.LastOrDefault(i => i.FkProductID == item.ProductID);
                int saiId = 0;
                if (sellActionItem != null)
                {
                    saiId = sellActionItem.SellActionItemID; 
                }
				var vm = new SliderViewModel
                {
                    ImageUrl = item.ImageUrl,
                    SlugUrl = $"{artikel.ProductID}-{artikel.ProductNumber}-{FriendlyUrlHelper.ReplaceUmlaute(artikel.Name)}",
                    SellActionItemId = saiId
                };
				urls.Add(vm);
			}
			
			model.SliderViews = urls;
			model.StartPage = startpage;
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Fargen oder Anregungen";

            return View();
        }

		public IActionResult Privacy()
		{
			var dsgvo = _context.ShopFiles.Where(sf => sf.IsActive && sf.ShopFileType == Enums.ShopFileTypeEnum.DSK).Single();
			ViewData["DSGVO"] = dsgvo.Filename;
			return View();
		}
		//TermsAndConditions
		public IActionResult TermsAndConditions(Guid ?id)
		{
			ViewData["BackToOrderCartId"] = null;
			if (id != Guid.Empty)
			{
				ViewData["BackToOrderCartId"] = id;
			}
			var agb = _context.LawContents.Single(l => l.ID == 1);
			return View(agb);
		}

		public IActionResult Cancellation(Guid ?id)
		{
			ViewData["BackToOrderCartId"] = null;
			if (id != Guid.Empty)
			{
				ViewData["BackToOrderCartId"] = id;
			}
			var cancellation = _context.LawContents.Single(l => l.ID == 2);
			return View(cancellation);
		}

		public IActionResult PrivacyPolicy()
		{
			var privacypolicy = _context.LawContents.Single(l => l.ID == 3);
			return View(privacypolicy);
		}

		public IActionResult Imprint()
		{
			var imprint = _context.LawContents.Single(l => l.ID == 4);
			return View(imprint);
		}


		public IActionResult PleaseConfirmEmail()
		{
			return View();
		}

		public IActionResult PaymendAndShipping()
		{
			return View();
		}

		public IActionResult Faq()
		{
			return View();
		}

		public IActionResult OnWeb()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
