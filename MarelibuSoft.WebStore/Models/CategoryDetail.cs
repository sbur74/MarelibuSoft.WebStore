using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class CategoryDetail
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public int CategorySubID { get; set; }
		[Display(Name = "Unterkategorie")]
		public CategorySub Sub { get; set; }
	}
}
