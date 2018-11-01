using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class ProductDetailViewModel
    {
		[Key]
		public int ProductID { get; set; }
		[Display(Name = "Artikelnr.")]
		public int ProductNumber { get; set; }
		public string Name { get; set; }
		[Display(Name = "Kurzbeschreibung")]
		public string ShortDescription { get; set; }
		[Display(Name = "Beschreibung")]
		//[DisplayFormat(HtmlEncode = true)]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
		[Display(Name = "Grundpreis")]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }
		[Display(Name = "Verfügbare Menge")]
		public decimal AvailableQuantity { get; set; }
		[Display(Name = "Mindestabnahme")]
		public decimal MinimumPurchaseQuantity { get; set; }
		[Display(Name = "Einheit")]
		public string BasesUnit { get; set; }
		public int BasesUnitID { get; set; }
		[Display(Name = "Größe")]
		public string Size { get; set; }
		[Display(Name = "Lieferzeit")]
		public string ShippingTime { get; set; }
		[Display(Name = "Sekundäre Preis")]
		public string SecondPriceUnit { get; set; }

		[Display(Name = "SEO Beschreibung")]
		[StringLength(155, ErrorMessage = "Du darfst nicht mehr als 155 Zeichen vervenden!")]
		public string SeoDescription { get; set; }

		[Display(Name = "SEO Schlagworte(Keywords)")]
		public string SeoKeywords { get; set; }

		//TODO: hide this fields in view
		public int CategoryID { get; set; }
		public int CategorySubID { get; set; }
		public int CategoryDetailID { get; set; }

		public ProductImage MainImageUrl { get; set; }

		public string SlugUrl { get; set; }

		public List<ProductImage> ImageUrls { get; set; }
	}
}
