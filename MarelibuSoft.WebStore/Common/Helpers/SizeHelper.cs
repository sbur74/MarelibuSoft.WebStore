using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class SizeHelper
    {
		private readonly ApplicationDbContext _context;
		public SizeHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<SizeViewModel> GetVmSizes()
		{
			List<SizeViewModel> vmsizes = new List<SizeViewModel>() { new SizeViewModel { ID = 0, Name = "Keine" } };

			var sizes = _context.Sizes;

			foreach (Size item in sizes)
			{
				SizeViewModel vm = new SizeViewModel { ID = item.SizeID, Name = item.Name };
				vmsizes.Add(vm);
			}

			return vmsizes;
		}

		public string GetName(int id)
		{
			string name = string.Empty;

			if (id != 0)
			{
				name = _context.Sizes.Single(s => s.SizeID == id).Name; 
			}

			return name;
		}
    }
}
