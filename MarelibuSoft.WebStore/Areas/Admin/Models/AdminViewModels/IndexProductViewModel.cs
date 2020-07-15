using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class IndexProductViewModel
    {
		[Key]
		public int ProductID { get; set; }
		[Display(Name = "Artikelnummer")]
		public int ProductNumber { get; set; }
		public string Name { get; set; }
		[Display(Name = "Wesendliche Merkmale")]
		public string ShortDescription { get; set; }
		[DataType(DataType.MultilineText)]
		[Display(Name = "Beschreibung")]
		public string Description { get; set; }
		[DataType(DataType.Currency)]
		[Display(Name = "Preis")]
		public decimal Price { get; set; }
		[Display(Name = "Verfügbaremenge")]
		public decimal AvailableQuantity { get; set; }
		//[Display(Name = "Mindestabnahmemenge")]
		//public string MinimumPurchaseQuantity { get; set; }
		[Display(Name = "Basiseinheit")]
		public string BasesUnit { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Lieferzeit")]
		public string ShippingTime { get; set; }
		[Display(Name = "Lieferkostentyp")]
		public string ShippingPriceTypeName { get; set; }
		[Display(Name = "Aktiv")]
		public bool IsActive { get; set; }
		public string MainImage { get; set; }
	}
}
