using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ManageViewModels
{
    public class IndexViewModel
    {
		[Display(Name = "Benutzername")]
        public string Username { get; set; }
		[Display(Name = "EMail bestätigt")]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
		[Display(Name = "Statusmeldung")]
        public string StatusMessage { get; set; }
		[Display(Name = "Kundennummer")]
		public string CustomerNo { get; set; }

		[Required(ErrorMessage = "Ein Vorname muss eingeben werden")]
		[Display(Name = "Vorname")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ein Nachname muss eingeben werden")]
		[Display(Name = "Nachname")]
		public string Name { get; set; }

		[Display(Name = "Firmenname")]
		public string CompanyName { get; set; }

		[Required(ErrorMessage = "Straße und Hausnummer muss eingeben werden")]
		[Display(Name = "Straße, Nr.")]
		public string Address { get; set; }

		[Display(Name = "Adresszusatz")]
		public string AdditionalAddress { get; set; }

		[Required(ErrorMessage = "Ein Ort muss eingeben werden")]
		[Display(Name = "Ort")]
		public string City { get; set; }

		[Required(ErrorMessage = "Eine PLZ muss eingeben werden")]
		[Display(Name = "PLZ")]
		public string PostCode { get; set; }
		
		[Required(ErrorMessage = "Ein Land muss ausgewählt werden")]
		[Display(Name = "Land")]
		public int CountryID { get; set; }
		public string CountryName { get; set; }

		public List<Country> Countries { get; set; }

		public Guid CustomerID { get; set; }
	}
}
