using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class CategoryAssignmentMultiViewModel
    {
		public int ID { get; set; }
		[Display(Name = "Artikel")]
		public int ProductID { get; set; }
		[Display(Name = "Kategorie")]
		public List<int> Categories { get; set; }
		[Display(Name = "Unterkategorie")]
		public List<int> CategorySubs { get; set; }
		[Display(Name = "Detail-Kategorie")]
		public List<int> CategoryDetails { get; set; }
	}
}
