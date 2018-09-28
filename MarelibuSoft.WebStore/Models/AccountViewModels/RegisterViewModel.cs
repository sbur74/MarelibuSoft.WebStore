using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Die EMail Adresse darf nicht leer sein.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Das Passwort darf nicht leer sein.")]
        [StringLength(100, ErrorMessage = "Das Passwort muss mindestens {0} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Passwort wiederholen")]
        [Compare("Password", ErrorMessage = "Die eingebenen Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; }

		//Customer
		[Required(ErrorMessage = "Ein Vorname muss eingeben werden")]
		[Display(Name = "Vorname")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Ein Nachname muss eingeben werden")]
		[Display(Name = "Nachname")]
		public string Name { get; set; }

		[Display(Name = "Firmenname")]
		public string CompanyName { get; set; }

		[Required(ErrorMessage = "Straße und Hausnummer muss eingeben werden")]
		[Display(Name = "Straße, Hausnummer")]
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

		public string ShoppingCartId { get; set; }
	}
}
