using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Customer
    {
		public Guid CustomerID { get; set; }
		public string CustomerNo { get; set; }
		public string FirstName { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string AdditionalAddress { get; set; }
		public string City { get; set; }
		public string PostCode { get; set; }
		
		[Display(Name = "Darf auf Rechnung kaufen")]
		public bool AllowedPayByBill { get; set; }

		public string UserId { get; set; }

		public int CountryId { get; set; }
	}
}
