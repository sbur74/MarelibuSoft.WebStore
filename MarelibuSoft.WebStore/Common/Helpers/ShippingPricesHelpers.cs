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
		private CountryHelper countryHelper;

		public ShippingPricesHelpers(ApplicationDbContext dbContext)
		{
			context = dbContext;
			countryHelper = new CountryHelper(context);
		}

		public async Task<List<ShippingPricesViewModel>> GetShippingPricesViewModels(int priceTypeId)
		{
			List<ShippingPricesViewModel> resultList = new List<ShippingPricesViewModel>();

			var shipprices = await context.ShippingPrices.Where(t => t.ShippingPriceTypeId == priceTypeId).ToListAsync();

			foreach (var item in shipprices)
			{
				bool allow = await countryHelper.GetAllowedForShipping(item.CountryId);
				if (allow)
				{
					var vm = new ShippingPricesViewModel
					{
						Id = item.ID,
						CountryId = item.CountryId,
						CountryName = countryHelper.GetNameByID(item.CountryId),
						Name = item.Name,
						Price = item.Price,
						ShippingPriceTypeId = item.ShippingPriceTypeId
					};
					resultList.Add(vm);
				}

				
			}

			return resultList;
		}
	}
}
