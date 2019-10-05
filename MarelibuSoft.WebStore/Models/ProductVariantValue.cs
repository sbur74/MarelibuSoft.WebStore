using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ProductVariantValue
    {
        public int Id { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
