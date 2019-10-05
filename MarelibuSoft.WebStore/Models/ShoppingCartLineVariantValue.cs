using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShoppingCartLineVariantValue
    {
        public int Id { get; set; }
        public int ProductVariantOption { get; set; }
        public int ProductVariant { get; set; }
        public string Value { get; set; }
        public string Combi { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price{ get; set; }
        public ShoppingCartLine CartLine { get; set; }
        public int ShoppingCartLineId { get; set; }
    }
}
