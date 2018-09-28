using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
	public class ShippingPricesViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }

		public int ShippingPriceTypeId { get; set; }
	}
}
