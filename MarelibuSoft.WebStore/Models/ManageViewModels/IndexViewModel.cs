using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
		[Display(Name = "Kundennummer")]
		public string CustomerNo { get; set; }

		[Required]
		[Display(Name = "Vorname")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Nachname")]
		public string Name { get; set; }
		[Display(Name = "Adresse")]
		public string Address { get; set; }
		[Display(Name = "Adressezusatz")]
		public string AdditionalAddress { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Display(Name = "PLZ")]
		public string PostCode { get; set; }
		[Display(Name = "Land")]
		public string CountryName { get; set; }
		public int CountryID { get; set; }
		[Display(Name ="Geburtsdatum")]
		public DateTime DateOfBirth { get; set; }

		public Guid CustomerID { get; set; }
	}
}
