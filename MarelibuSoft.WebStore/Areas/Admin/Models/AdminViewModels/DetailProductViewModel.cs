using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class DetailProductViewModel
    {
		[Key]
		public int ProductID { get; set; }
		[Display(Name="Artikelnummer")]
		public int ProductNumber { get; set; }
		public string Name { get; set; }
		[Display(Name = "Kurzbeschreibung")]
		public string ShortDescription { get; set; }
		[Display(Name = "Beschreibung")]
		public string Description { get; set; }
		[Display(Name = "Preis")]
		public string Price { get; set; }
		[Display(Name = "Verfügbaremenge")]
		public string AvailableQuantity { get; set; }
		[Display(Name = "Mindestabnahmemenge")]
		public string MinimumPurchaseQuantity { get; set; }
		public string BasesUnit { get; set; }
		[Display(Name = "Größe")]
		public string Size { get; set; }
		public string SecondBaseUnit { get; set; }
		[Display(Name = "Sekundäre Preis")]
		public string SecondBasePrice { get; set; }
		[Display(Name = "Lieferzeit")]
		public string ShippingPeriod { get; set; }
		[Display(Name = "SEO Beschreibung")]
		[StringLength(155, ErrorMessage = "Du darfst nicht mehr als 155 Zeichen vervenden!")]
		public string SeoDescription { get; set; }
		[Display(Name = "SEO Schlagworte(Keywords)")]
		public string SeoKeywords { get; set; }
		public List<string> ImageUrls { get; set; }
	}
}
