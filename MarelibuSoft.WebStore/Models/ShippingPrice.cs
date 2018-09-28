using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShippingPrice
    {
		public int ID { get; set; }
		public String Name { get; set; }
		public Decimal Price { get; set; }
		public int CountryId { get; set; }

		public int ShippingPriceTypeId { get; set; }
		public ShippingPriceType ShippingPriceType { get; set; }
	}
}
