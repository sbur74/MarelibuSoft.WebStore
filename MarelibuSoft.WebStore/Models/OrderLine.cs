using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class OrderLine
    {
		public int OrderLineID { get; set; }
		public int Position { get; set; }
		public int ProductID { get; set; }
		public decimal Quantity { get; set; }
		public decimal SellBasePrice { get; set; } // price per unit at selling time
		public int UnitID { get; set; }
        public int ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }

        public Guid OrderID { get; set; }
		public Order Order { get; set; }
       

        public List<OrderLineVariantValue> VariantValues { get;  set; }
        public List<OrderLineTextOption> OrderLineTextOptions { get; set; }
    }
}
