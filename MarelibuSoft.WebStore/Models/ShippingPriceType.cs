using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShippingPriceType
    {
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<ShippingPrice> Prices { get; set; }
	}
}
