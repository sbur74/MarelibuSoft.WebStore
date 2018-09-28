using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class ProductHelper
    {
		private readonly ApplicationDbContext _context;
		private readonly ILogger _logger;
		private readonly ILoggerFactory factory;

		public ProductHelper(ApplicationDbContext context, ILoggerFactory loggerFactory)
		{
			_context = context;
			factory = loggerFactory;
			_logger = factory.CreateLogger<ProductHelper>();
		}

		public List<SelectProductViewModel> GetSelectVmList()
		{
			List<SelectProductViewModel> list = null;

			var products = _context.Products;

			if (products != null)
			{
				list = new List<SelectProductViewModel>();
				foreach (var item in products)
				{
					var vm = new SelectProductViewModel { ID = item.ProductID, No = item.ProductNumber, Name = item.Name, Img = new ProductImageHelper(_context, factory).GetMainImageUrl(item.ProductID) };
					list.Add(vm);
				} 
			}

			return list;
		}
	}
}
