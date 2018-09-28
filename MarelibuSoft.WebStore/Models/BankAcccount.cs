using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class BankAcccount
    {
		public int ID { get; set; }
		[Display(Name = "Kontoinhaber")]
		public string AccountOwner { get; set; }
		[Display(Name = "Bank")]
		public string Institute { get; set; }
		[Display(Name = "IBAN")]
		public string Iban { get; set; }
		[Display(Name = "Swift-Bic")]
		public string SwiftBic { get; set; }
	}
}
