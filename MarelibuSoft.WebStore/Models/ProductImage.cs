using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ProductImage
    {
		public int ProductImageID { get; set; }
		public string Name { get; set; }
		public bool IsMainImage { get; set; }
		public string ImageUrl { get; set; }

		public int ProductID { get; set; }
		public Product Product { get; set; }
	}
}
