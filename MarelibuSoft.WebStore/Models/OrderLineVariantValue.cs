using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class OrderLineVariantValue
    {
        public int Id { get; set; }
        public int ProductVariant { get; set; }
        public int ProductVariantOption { get; set; }
        public string VarinatName { get; set; }
        public string Value { get; set; }
        public string Combi { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderLineId { get; set; }
        public OrderLine OrderLine { get; set; }
    }
}
