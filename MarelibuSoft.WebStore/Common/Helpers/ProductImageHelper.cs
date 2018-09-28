using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class ProductImageHelper
    {
		private readonly ApplicationDbContext _context;
		public ProductImageHelper(ApplicationDbContext context)
		{
			_context = context;
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
			string url = string.Empty;
			var img = _context.ProductImages.Where(i => i.IsMainImage && i.ProductID == id).First();

			if (img != null)
			{
				url = img.ImageUrl;
			}

			return url;
		}
    }
}
