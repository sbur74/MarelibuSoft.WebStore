using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class ProductImageHelper
    {
		private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;
		private readonly ILoggerFactory factory;

		public ProductImageHelper(ApplicationDbContext context, ILoggerFactory logger)
		{
			_context = context;
			factory = logger;
			_logger = factory.CreateLogger<ProductImageHelper>();
		}

		public List<string>GetUrls(int id)
		{
			List<string> urls = new List<string>();

			var images = _context.ProductImages.Where(i => i.ProductID == id).ToList();

			foreach (ProductImage item in images)
			{
				string url = item.ImageUrl;
				urls.Add(url);
			}
			return urls;
		}

		public string GetMainImageUrl(int id)
		{
			string url = "noImage.svg";
			ProductImage img = null;

			try
			{
				img =_context.ProductImages.Where(i => i.IsMainImage && i.ProductID == id).First();
			} 
			catch (Exception e)
			{
				_logger.LogError(e, "ProductImageHelper.GetMainImageUrl --> Fehler beim suchen des Hauptproduktbildes");
			}
			try
			{
				img = _context.ProductImages.Where(i => i.ProductID == id).First();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "ProductImageHelper.GetMainImageUrl --> Fehler beim suchen eines Produktbildes");
			}

			if (img != null)
			{
				url = img.ImageUrl;
			}

			return url;
		}
    }
}
