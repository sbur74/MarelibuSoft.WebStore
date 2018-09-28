using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class ShippingAddressViewModel
    {
		public int ID { get; set; }
		[Required(ErrorMessage = "Ein Vorname muss eingeben werden")]
		[Display(Name = "Vorname")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ein Nachname muss eingeben werden")]
		[Display(Name = "Name")]
		public string LastName { get; set; }

		[Display(Name = "Firmenname")]
		public string CompanyName { get; set; }

		[Required(ErrorMessage = "Straße und Hausnummer muss eingeben werden")]
		[Display(Name = "Straße, Haus-Nr.")]
		public string Address { get; set; }

		[Display(Name = "Adresszusatz")]
		public string AdditionalAddress { get; set; }

		[Required(ErrorMessage = "Ein Ort muss eingeben werden")]
		[Display(Name = "Ort")]
		public string City { get; set; }

		[Required(ErrorMessage = "Eine PLZ muss eingeben werden")]
		[Display(Name = "PLZ")]
		public string PostCode { get; set; }

		[Display(Name = "Lieferadresse verwenden")]
		public bool IsMainAddress { get; set; }
		[Display(Name = "Rechnungsadresse")]
		public bool IsInvoiceAddress { get; set; }

		public Guid CustomerID { get; set; }
		[Required(ErrorMessage = "Ein Land muss ausgewählt werden")]
		public int CountryID { get; set; }
		[Display(Name = "Land")]
		public string CountryName { get; set; }
	}
}
