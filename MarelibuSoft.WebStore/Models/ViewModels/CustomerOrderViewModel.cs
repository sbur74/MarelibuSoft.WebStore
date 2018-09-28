using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
	public class CustomerOrderViewModel
	{
		public Guid ID { get; set; }
		[Display(Name = "Auftragsnummer")]
		public string Number { get; set; }
		[Display(Name = "Auftragsdatum")]
		public DateTime OrderDate { get; set; }
		[Display(Name = "Zahlungsart")]
		public string PaymentName { get; set; }
		[Display(Name = "Rechung bezahlt")]
		public bool IsPayed { get; set; }
		[Display(Name = "Ware versendet")]
		public bool IsSend { get; set; }
		[Display(Name = "Versanddatum")]
		public DateTime Shippingdate { get; set; }
		[Display(Name = "Auftrag abgeschlossen")]
		public bool IsClosed { get; set; }
		[Display(Name = "Ich habe die Rechtlichenhinweise gelesen")]
		public bool ExceptLawConditions { get; set; }

		public Guid CustomerID { get; set; }

		[Display(Name = "Versandkosten")]
		public Decimal ShipPrice { get; set; }
		[Display(Name = "Versandzeitraum")]
		public string ShipPeriod { get; set; }
		[Display(Name = "Gesamtbetrag in Euro")]
		public decimal Total { get; set; }
		[Display(Name = "Zusätzliche Angaben")]
		public string FreeText { get; set; }

		[Display(Name = "Lieferadresse")]
		public string ShippingAddressName { get; set; }
		[Display(Name = "Lieferadresse-Str.")]
		public string ShippingAddressString { get; set; }
		[Display(Name = "Lieferadresse-Ort")]
		public string ShippingPostCodeCity { get; set; }
		[Display(Name = "Lieferadresse-Land")]
		public string ShippingCountryName { get; set; }

		[Display(Name = "Rechnungsadresse")]
		public string InvoiceAddressName { get; set; }
		[Display(Name = "Rechnungsadresse-Str.")]
		public string InvoiceAddressString { get; set; }
		[Display(Name = "Rechnungsadresse-Ort")]
		public string InvoicePostCodeCity { get; set; }
		[Display(Name = "Rechnungsadresse-Land")]
		public string InvoiceCountryName { get; set; }



		public List<CustomerOrderLineViewModel> Lines { get; set; }
	}
}
