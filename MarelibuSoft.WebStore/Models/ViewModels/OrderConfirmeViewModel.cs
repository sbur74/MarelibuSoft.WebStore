using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class OrderConfirmeViewModel
    {
		public Guid ID { get; set; }
		public string Number { get; set; }
		public int CustomerID { get; set; }
		public int GustID { get; set; }
	}
}
