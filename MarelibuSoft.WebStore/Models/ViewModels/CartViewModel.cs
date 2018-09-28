using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class CartViewModel
    {
		public Guid ID { get; set; }
		public string Number { get; set; }
		public Guid OrderId { get; set; }
		public decimal Total { get; set; }
		public int DefaultCountry { get; set; }

		public string ShippingPeriodName { get; set; }

		public List<ShippingPricesViewModel> ShipPrices { get; set; }

		public List<CartLineViewModel> Lines { get; set; }
	}
}
