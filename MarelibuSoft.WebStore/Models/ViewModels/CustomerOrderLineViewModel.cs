using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
	public class CustomerOrderLineViewModel
	{
		public int OrderLineID { get; set; }
		[Display(Name = "Position")]
		public int Position { get; set; }
		public int ProductID { get; set; }
		[Display(Name = "Artikel-Nr.")]
		public string ProductNumber { get; set; }
		[Display(Name = "Artikel")]
		public string ProductName { get; set; }
		[Display(Name = "Gekaufte Menge")]
		public decimal Quantity { get; set; }
		[Display(Name ="Kaufpreis")]
		public decimal SellBasePrice { get; set; } // price per unit at selling time
		[Display(Name = "Einheit")]
		public string Unit { get; set; }

		public Guid OrderID { get; set; }

        public List<TextOptionViewModel> TextOptions { get; set; }
        public List<VariantViewModel> VariantList { get; set; }
    }
}
