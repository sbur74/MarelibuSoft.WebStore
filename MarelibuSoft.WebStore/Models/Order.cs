using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Order
    {
		public Guid ID { get; set; }
		[Display(Name = "Auftragsnummer")]
		public string Number { get; set; }
		[Display(Name = "Auftragsdatum")]
		public DateTime OrderDate { get; set; }
		[Display(Name = "Zahlungsart")]
		public int PaymentID { get; set; }
		[Display(Name = "Rechung bezahlt")]
		public bool IsPayed { get; set; }
		[Display(Name ="Ware versendet")]
		public bool IsSend { get; set; }
		[Display(Name = "Versanddatum")]
		public DateTime Shippingdate { get; set; }
		[Display(Name = "Lieferadresse")]
		public int ShippingAddressId { get; set; }
		[Display(Name = "Auftrag abgeschlossen")]
		public bool IsClosed { get; set; }
		public Guid CartID { get; set; }
		public Guid CustomerID { get; set; }
		public Guid GustID { get; set; }
		[Display(Name = "Ich habe die Rechtlichenhinweise gelesen")]
		[Required]
		public bool ExceptLawConditions { get; set; }
		[Display(Name = "Auftragspositionen")]
		public List<OrderLine> OderLines { get; set; }
		public int ShippingPriceId { get; set; }
		[Display(Name = "Versandkosten")]
		public Decimal ShippingPrice { get; set; }
		[Display(Name = "Versandzeitraum")]
		public int ShippingPeriodId { get; set; }
		[Display(Name = "Gesamtbetrag")]
		public decimal Total { get; set; }
		[Display(Name = "Zusätzliche Angaben")]
		public string FreeText { get; set; }
	}
}
