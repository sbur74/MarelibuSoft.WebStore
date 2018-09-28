using MarelibuSoft.WebStore.Common.ViewModels;
using MarelibuSoft.WebStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class ShippingPriceTypeHelper : ISelectionHelper
    {
		private readonly ApplicationDbContext _context;
		public ShippingPriceTypeHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public string GetNameByID(int id)
		{
			string name = string.Empty;

			if (id > 0)
			{
				var shpty = _context.ShippingPriceTypes.Single(s => s.ID == id);

				if (shpty != null)
				{
					name = shpty.Name;
				} 
			}

			return name;
		}

		public List<SelectItemViewModel> GetVmList()
		{
			List<SelectItemViewModel> selectItemViewModels = new List<SelectItemViewModel>();
			var list = _context.ShippingPriceTypes.ToList();

			SelectItemViewModel none = new SelectItemViewModel { ID = 0, Name = "nicht zugewiesen" };
			selectItemViewModels.Add(none);

			foreach (var item in list)
			{
				SelectItemViewModel select = new SelectItemViewModel { ID = item.ID, Name = item.Name };
				selectItemViewModels.Add(select);
			}

			return selectItemViewModels;
		}
	}
}
