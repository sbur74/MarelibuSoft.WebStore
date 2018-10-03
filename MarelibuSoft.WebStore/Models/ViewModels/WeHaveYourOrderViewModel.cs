using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class WeHaveYourOrderViewModel
	{ 
		[Key]
		public Guid OrderID { get; set; }
		[Display(Name = "Auftragsdatum")]
		public DateTime OrderDate { get; set; }
		[Display(Name = "Auftragsnummer")]
		public string OrderNo { get; set; }
		[Display(Name = "Auftragssumme")]
		public decimal OrderTotal { get; set; }
		[Display(Name = "Zahlungsart")]
		public string OrderPaymend { get; set; }
		[Display (Name = "Lieferzeitraum")]
		public string OrderShippingPeriod { get; set; }
		[Display(Name = "Hinweis")]
		public string OrderThankYou { get; set; }
		[Display(Name = "Bankverbindung")]
		public BankAcccount Bank { get; set; }
		[Display(Name = "Lieferadresse")]
		public ShippingAddressViewModel OrderShippingAddress { get; set; }
		[Display(Name = "Rechnungsadresse")]
		public ShippingAddressViewModel OrderInvoiceAddress { get; set; }
		public List<WeHaveYourOrderLineViewModel> OrderLines { get; set; }
		public string ShipPriceName { get; set; }
		public decimal ShipPrice { get; set; }
		[Display(Name = "Zusätzliche Angaben")]
		public string FreeText { get; set; }
	}
}
