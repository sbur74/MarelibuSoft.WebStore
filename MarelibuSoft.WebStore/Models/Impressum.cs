using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Impressum
    {
		public int ID { get; set; }
		public string ShopName { get; set; }
		public string ShopAdmin { get; set; }
		public string Address { get; set; }
		public string AdditionalAddress { get; set; }
		public string City { get; set; }
		public string PostCode { get; set; }

		public int CountryID { get; set; }

		public string Bank { get; set; }
		public string Iban { get; set; }
		public string Bic { get; set; }
		public string EMail { get; set; }
	}
}