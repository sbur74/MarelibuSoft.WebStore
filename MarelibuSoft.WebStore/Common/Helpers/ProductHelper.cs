using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class ProductHelper
    {
		private readonly ApplicationDbContext _context;

		public ProductHelper(ApplicationDbContext context)
		{
			_context = context;
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
					var vm = new SelectProductViewModel { ID = item.ProductID, No = item.ProductNumber, Name = item.Name, Img = new ProductImageHelper(_context).GetMainImageUrl(item.ProductID) };
					list.Add(vm);
				} 
			}

			return list;
		}
	}
}
