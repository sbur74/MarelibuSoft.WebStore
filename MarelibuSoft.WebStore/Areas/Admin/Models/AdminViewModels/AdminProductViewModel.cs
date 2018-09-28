using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class AdminProductViewModel
    {
		[Key]
		public int ProductID { get; set; }
		[Display(Name = "Artikelnummer")]
		public int ProductNumber { get; set; }
		[Required]
		public string Name { get; set; }
		[Display(Name = "Wesendliche Merkmale")]
		public string ShortDescription { get; set; }
		[DataType(DataType.MultilineText)]
		[Display(Name = "Beschreibung")]
		public string Description { get; set; }
		[DataType(DataType.Currency)]
		[Display(Name = "Preis")]
		public string Price { get; set; }
		[Display(Name = "Verfügbaremenge")]
		public string AvailableQuantity { get; set; }
		[Display(Name = "Mindestabnahmemenge")]
		public string MinimumPurchaseQuantity { get; set; }
		[Display(Name = "Preiseinheit")]
		public string BasesUnit { get; set; }
		public int BasesUnitID { get; set; }		
		[Display(Name = "Größe")]
		public string Size { get; set; }
		public int SizeID { get; set; }
		[Display(Name = "Lieferzeit")]
		public string Period { get; set; }
		public int PeriodID { get; set; }
		[Display(Name = "Lieferkostentyp")]
		public string ShippingPriceTypeName { get; set; }
		[Display(Name = "Lieferkostentyp Id")]
		public int ShippingPriceTypeID { get; set; }
		[Display(Name = "Sekundäre Preiseinheit")]
		public string SecondBaseUnit { get; set; }
		public int SecondBaseUnitID { get; set; }
		[Display(Name ="Sekundäre Preis")]
		public string SecondBasePrice { get; set; }
		[Display(Name = "Kategorie")]
		public string CategoryName { get; set; }
		public int CategoryID { get; set; }
		[Display(Name = "Unterkategorie")]
		public string CategorySubName { get; set; }
		public int CategorySubID { get; set; }
		[Display(Name = "Detail-Kategorie")]
		public string CategoryDetailName { get; set; }
		public int CategoryDetailID { get; set; }
		[Display(Name = "Aktiv")]
		public bool IsActive { get; set; }

		public List<string> ImageUrls { get; set; }
	}
}
