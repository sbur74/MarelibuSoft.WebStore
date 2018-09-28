using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class CategoryAssignmentViewModel
    {
		public int ID { get; set; }
		[Display(Name = "Artikelname")]
		public String ProductName { get; set; }
		[Display(Name = "Arikelnummer")]
		public int ProductNo { get; set; }
		public string ProductImage { get; set; }
		[Display(Name = "Kategorie")]
		public string Category { get; set; }
		[Display(Name = "Unterkategorie")]
		public string CategorySub { get; set; }
		[Display(Name = "Detail-Kategorie")]
		public string CategoryDetail { get; set; }
	}
}
