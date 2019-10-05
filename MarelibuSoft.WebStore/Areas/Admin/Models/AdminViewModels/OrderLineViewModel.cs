using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Models.ViewModels;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class OrderLineViewModel
    {
		[Key]
		public int ID { get; set; }
		public int Position { get; set; }
		[Display(Name = "Verkaufspreis")]
		public decimal OrderPrice { get; set; }
		[Display(Name = "Gekaute Menge")]
		public decimal OrderQuantity { get; set; }
		[Display(Name = "Betrag")]
		public decimal OrderLineTotal { get; set; }
		[Display(Name ="Artikel")]
		public string ProductName { get; set; }
		[Display(Name = "Artikelnummer")]
		public int ProductNumber { get; set; }
		[Display(Name = "Artikelbild")]
		public string Image { get; set; }
		[Display(Name = "Verkauseinheit")]
		public string OrderUnit { get; set; }

        public List<TextOptionViewModel> TextOptionList { get; set; }
        public List<VariantViewModel> VariantList { get; set; }
    }
}
