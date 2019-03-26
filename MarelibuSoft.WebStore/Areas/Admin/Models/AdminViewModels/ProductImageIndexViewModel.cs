using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
	public class ProductImageIndexViewModel
	{
		public int ProductImageID { get; set; }
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public string ProductName { get; set; }
		public bool IsMainImage { get; set; }
	}
}
