using MarelibuSoft.WebStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
	public class AdminCustomerViewModel
	{
		[Display(Name = "Kunden ID")]
		public Guid CustomerID { get; set; }
		[Display(Name = "Kunden Nummer")]
		public string CustomerNo { get; set; }
		[Display(Name = "Vorname")]
		public string FirstName { get; set; }
		public string Name { get; set; }
		[Display(Name = "Straße, Haus-Nr.")]
		public string Address { get; set; }
		[Display(Name = "Adresszusatz")]
		public string AdditionalAddress { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Display( Name = "PLZ")]
		public string PostCode { get; set; }

		[Display(Name = "Benutzer")]
		public string UserEmail { get; set; }

		[Display(Name = "Darf auf Rechnung kaufen")]
		public bool AllowedPayByBill { get; set; }

		public string UserId { get; set; }
		[Display(Name = "Land")]
		public int CountryId { get; set; }
		public string CountryName { get; set; }



		[Display(Name ="Lieferadressen")]
		public List<ShippingAddressViewModel> Addresses{ get; set; }
	}
}
