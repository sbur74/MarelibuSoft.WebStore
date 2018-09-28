using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	public class UnitHelper
    {
		private readonly ApplicationDbContext _context;

		public UnitHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<UnitViewModel> GetVmUnits()
		{
			List<UnitViewModel> vmunits = new List<UnitViewModel>() { new UnitViewModel { UnitID = 0, Name = "Keine" } };

			var units = _context.Units;

			foreach (Unit item in units)
			{
				UnitViewModel vm = new UnitViewModel { UnitID = item.UnitID, Name = item.Name };
				vmunits.Add(vm);
			}
			return vmunits;
		}

		public string GetUnitName(int id)
		{
			string name = string.Empty;

			if (id != 0)
			{
				name = _context.Units.Single(u => u.UnitID == id).Name; 
			}

			return name;
		}
	}
}
