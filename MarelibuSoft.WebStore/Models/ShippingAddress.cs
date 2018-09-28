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
		[Display(Name = "Adresse")]
		public string Address { get; set; }
		[Display(Name = "Adresszusatz")]
		public string AdditionalAddress { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Display(Name = "PLZ")]
		public string PostCode { get; set; }
		[Display(Name = "als Hauptlieferadresse verwenden")]
		public bool IsMainAddress { get; set; }
		//fk 
		public Guid CustomerID { get; set; }
		public int CountryID { get; set; }
	}
}
