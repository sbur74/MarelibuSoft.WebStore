using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Common.ViewModels;
using MarelibuSoft.WebStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class CategorySubHelper
    {
		private readonly ApplicationDbContext _context;

		public CategorySubHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public string GetNameByID(int ID)
		{
			string name = string.Empty;

			if (ID > 0)
			{
				var cat = _context.CategorySubs.Single(c => c.ID == ID);

				if (cat != null)
				{
					name = cat.Name;
				}
			}

			return name;
		}

		public List<SelectItemViewModel> GetVmList()
		{
			List<SelectItemViewModel> list = new List<SelectItemViewModel>() { new SelectItemViewModel { ID = "0", Name = "Keine Zuordnung" } };
			var categories = _context.CategorySubs.ToList();
			if (categories != null)
			{
				foreach (var item in categories)
				{
					var lItem = new SelectItemViewModel { ID = item.ID.ToString(), Name = item.Name };
					list.Add(lItem);
				}
			}
			return list;
		}
	}
}
