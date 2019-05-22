using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class OrderCreateViewModel
    {
		public Order Order { get; set; }

		public CartViewModel Cart { get; set; }

		public ShippingPeriod ShippingPeriod { get; set; }

		public ShippingPrice ShippingPrice { get; set; }

        public string AGB { get; set; }

		public string Wdbl { get; set; }

		public string DSGVO { get; set; }

		public string PayPalTotal { get; set; }

		public bool CanBuyWithBill { get; set; }

		public List<ShippingAddressViewModel> ShippingAddresseVMs { get; set; }

		public List<ShippingPrice> ShippingPrices { get; set; }
	}
}
