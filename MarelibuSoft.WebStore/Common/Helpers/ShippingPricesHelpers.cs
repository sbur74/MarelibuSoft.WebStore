using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	public class ShippingPricesHelpers
	{
		private readonly ApplicationDbContext context;

		public ShippingPricesHelpers(ApplicationDbContext dbContext)
		{
			context = dbContext;
		}

		public async Task<List<ShippingPricesViewModel>> GetShippingPricesViewModels(int priceTypeId)
		{
			List<ShippingPricesViewModel> resultList = new List<ShippingPricesViewModel>();

			var shipprices = await context.ShippingPrices.Where(t => t.ShippingPriceTypeId == priceTypeId).ToListAsync();
			var countries = await context.Countries.ToListAsync();

			foreach (var item in shipprices)
			{
				var vm = new ShippingPricesViewModel
				{
					Id = item.ID,
					CountryId = item.CountryId,
					CountryName = countries.Single(c => c.ID == item.CountryId).Name,
					Name = item.Name,
					Price = item.Price,
					ShippingPriceTypeId = item.ShippingPriceTypeId
				};

				resultList.Add(vm);
			}

			return resultList;
		}
	}
}
