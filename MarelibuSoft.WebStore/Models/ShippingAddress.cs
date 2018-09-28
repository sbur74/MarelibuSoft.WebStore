using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShippingAddress
    {
		public int ID { get; set; }
		[Display(Name = "Vorname")]
		public string FirstName { get; set; }
		[Display(Name = "Name")]
		public string LastName { get; set; }
		[Display(Name = "Firmenname")]
		public string CompanyName { get; set; }
		[Display(Name = "Adresse")]
		public string Address { get; set; }
		[Display(Name = "Adresszusatz")]
		public string AdditionalAddress { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Display(Name = "PLZ")]
		public string PostCode { get; set; }
		[Display(Name = "Lieferadresse verwenden")]
		public bool IsMainAddress { get; set; }
		[Display(Name = "Rechnungsadresse")]
		public bool IsInvoiceAddress { get; set; }
										  
		public Guid CustomerID { get; set; }
		[Display(Name = "Land")]
		[Required(ErrorMessage = "Es muss ein Land ausgewählt sein")]
		public int CountryID { get; set; }
	}
}
