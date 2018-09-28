using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Common.ViewModels;
using MarelibuSoft.WebStore.Data;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	public class CountryHelper : ISelectionHelper
	{
		private readonly ApplicationDbContext _context;
		public CountryHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public string GetNameByID(int id)
		{
			string name = string.Empty;

			if (id > 0)
			{
				var country = _context.Countries.Single(c => c.ID == id);
				if (country != null) name = country.Name;
			}

			return name;
		}

		public string GetCodeByID(int id)
		{
			string code = string.Empty;

			if (id > 0)
			{
				var country = _context.Countries.Single(c => c.ID == id);
				if (country != null) code = country.Code;
			}

			return code;
		}

		public List<SelectItemViewModel> GetVmList()
		{
			List<SelectItemViewModel> selectItemViewModels  = new List<SelectItemViewModel>();

			var coutries = _context.Countries.ToList();

			foreach (var item in coutries)
			{
				var vm = new SelectItemViewModel { ID = item.ID.ToString(), Name = item.Name };
				selectItemViewModels.Add(vm);
			}

			return selectItemViewModels;
		}

		public List<SelectItemViewModel> GetVmList(int id)
		{
			List<SelectItemViewModel> selectItemViewModels = new List<SelectItemViewModel>();

			var none = new SelectItemViewModel { ID = "", Name = "nicht zu gewiesen" };
			selectItemViewModels.Add(none);
			var coutries = _context.Countries.ToList();

			foreach (var item in coutries)
			{
				bool select = id == item.ID ? true : false;
				var vm = new SelectItemViewModel { ID = item.ID.ToString(), Name = item.Name, IsSelected = select };
				selectItemViewModels.Add(vm);
			}

			return selectItemViewModels;
		}
	}
}
