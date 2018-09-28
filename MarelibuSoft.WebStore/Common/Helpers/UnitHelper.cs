using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Data;
using Microsoft.Extensions.Logging;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	public class UnitHelper
    {
		private readonly ApplicationDbContext _context;
		private readonly ILoggerFactory factory;
		private readonly ILogger logger;

		public UnitHelper(ApplicationDbContext context, ILoggerFactory loggerFactory)
		{
			_context = context;
			factory = loggerFactory;
			logger = factory.CreateLogger<UnitHelper>();
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

			try
			{
				if (id != 0)
				{
					name = _context.Units.Single(u => u.UnitID == id).Name;
				}
			}
			catch (Exception e)
			{
				logger.LogError(e, "UnitHelper.GetUnitName");
			}

			return name;
		}
	}
}
