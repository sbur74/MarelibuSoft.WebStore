using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class ProductThumbnailsViewModel
    {
		[Key]
		public int ProductID { get; set; }
		[Display(Name = "Artikelnr.")]
		public int ProductNumber { get; set; }
		public string Name { get; set; }
		[Display(Name = "Kurzbeschreibung")]
		public string ShortDescription { get; set; }
		[Display(Name = "Beschreibung")]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
		[Display(Name = "Preis")]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }
		[Display(Name = "Verfügbaremenge")]
		public decimal AvailableQuantity { get; set; }
		[Display(Name = "Mindestabnahmemenge")]
		public decimal MinimumPurchaseQuantity { get; set; }
		[Display(Name = "Einheit")]
		public string BasesUnit { get; set; }
		public int BasesUnitID { get; set; }
		[Display(Name = "Größe")]
		public string Size { get; set; }
		[Display(Name = "Lieferzeit")]
		public string ShippingTime { get; set; }
		[Display(Name="Preiseinheit 2")]
		public string SecondPriceUnit { get; set; }
										   //TODO: hide this fields in view
		public int CategoryID { get; set; }
		public int CategorySubID { get; set; }
		public int CategoryDetailID { get; set; }

		public string MainImageUrl { get; set; }

		public List<string> ImageUrls { get; set; }
	}
}
