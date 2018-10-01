using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Country
    {
		public int ID { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		[Display(Name = "Für Lieferung zulassen")]
		public bool IsAllowedShipping { get; set; }
	}
}
