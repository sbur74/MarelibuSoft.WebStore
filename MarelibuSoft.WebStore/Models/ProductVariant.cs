using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class ProductVariant
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string OptionName { get; set; }

        public string CombiOptionName { get; set; }
        public bool IsAbsolutelyNecessary { get; set; }
        public List<ProductVariantOption> Options { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
