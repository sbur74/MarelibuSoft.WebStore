using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Product
    {
		public int ProductID { get; set; }
		public int ProductNumber { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ShortDescription { get; set; }
		public string SeoDescription { get; set; }
		public string SeoKeywords { get; set; }
		public decimal Price { get; set; }
		public decimal AvailableQuantity { get; set; }
		public decimal MinimumPurchaseQuantity { get; set; }

		public int BasesUnitID { get; set; }
		public int Size { get; set; }
		public int SecondBaseUnit { get; set; }
		public decimal SecondBasePrice { get; set; }
		public int ShippingPeriod { get; set; }
		public int ShippingPriceType { get; set; }
		public bool IsActive { get; set; }
        public DateTime PublishedOn { get; set; }

        public ICollection<CategoryAssignment> CategoryAssignments { get; set; }

		public List<ProductImage> ImageList { get; set; }
	}
}
