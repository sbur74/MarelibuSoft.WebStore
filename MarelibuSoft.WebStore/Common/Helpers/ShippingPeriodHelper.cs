using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
    public class ShippingPeriodHelper
    {
		private readonly ApplicationDbContext _context;
		public ShippingPeriodHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<ShippingPeriodViewModel> GetVmShippingPeriods()
		{
			List<ShippingPeriodViewModel> vmperiods = new List<ShippingPeriodViewModel>() { new ShippingPeriodViewModel { ID = 0, Value = "nicht zugewiesen" } };

			var periods = _context.ShpippingPeriods;

			foreach (ShippingPeriod item in periods)
			{
				ShippingPeriodViewModel vm = new ShippingPeriodViewModel { ID = item.ShippingPeriodID, Value = item.Decription };
				vmperiods.Add(vm);
			}

			return vmperiods;
		}

		public string GetDescription(int id)
		{
			string des = string.Empty;

			if (id != 0)
			{
				des = _context.ShpippingPeriods.Single(s => s.ShippingPeriodID == id).Decription; 
			}
			return des;
		}
    }
}
