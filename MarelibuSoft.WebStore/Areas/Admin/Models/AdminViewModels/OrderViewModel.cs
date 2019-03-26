using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class OrderViewModel
    {
		public Guid ID { get; set; }
		[Display(Name = "Auftragsnummer")]
		public string Number { get; set; }
		[Display(Name = "Auftragsdatum")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
		public DateTime OrderDate { get; set; }
		[Display(Name = "Zahlungsart")]
		public string Payment { get; set; }
		[Display(Name = "Rechung bezahlt")]
		public bool IsPayed { get; set; }
		[Display(Name = "Ware versendet")]
		public bool IsSend { get; set; }
		[Display(Name = "Versanddatum")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd}")]
		public DateTime Shippingdate { get; set; }
		[Display(Name = "Rechnungsadresse")]
		public ShippingAddressViewModel InvoiceAddress { get; set; }
		[Display(Name = "Versandadresse")]
		public ShippingAddressViewModel ShippToAddress { get; set; }
		[Display(Name = "Auftrag abgeschlossen")]
		public bool IsClosed { get; set; }
		[Display(Name ="E-Mail Adresse")]
		public string EMail { get; set; }
		public string CustomerFirstName { get; set; }
		public string CutomerLastName { get; set; }
		[Display(Name = "Ich habe die Rechtlichenhinweise gelesen")]
		[Required]
		public bool ExceptLawConditions { get; set; }
		[Display(Name = "Auftragspositionen")]
		public List<OrderLineViewModel> OderLines { get; set; }
		[Display(Name = "Versandpreisbeschreibung")]
		public string ShippingPriceName { get; set; }
		[Display(Name = "Versandkosten")]
		public Decimal ShippingPriceAtOrder { get; set; }
		[Display(Name = "Versandzeitraum")]
		public string ShippingPeriodString { get; set; }
		[Display(Name = "Gesamtbetrag in Euro")]
		public decimal Total { get; set; }
		[Display(Name = "Kundenwunsch")]
		public string FreeText { get; set; }
		[Display(Name = "Sendungsverfolungsnummer")]
		public string TrackingNumber { get; set; }

		public OrderStateViewModel StateViewModel { get; set; }
	}
}
