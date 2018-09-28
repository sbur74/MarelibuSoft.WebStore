using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class ShippingAddressViewModel
    {

		public Customer Customer { get; set; }
		public ShippingAddress ShippingAddress { get; set; }
		public Guest Guest { get; set; }
		public Order Order { get; set; }
	}
}
