using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShoppingCartLine
    {
		public int ID { get; set; }
		public int Position { get; set; }
		public int ProductID { get; set; }
		public decimal Quantity { get; set; }
		public decimal SellBasePrice { get; set; } // price per unit at selling time
		public int UnitID{ get; set; }
        public int SellActionItemId { get; set; }

        public List<ShoppingCartLineVariantValue> VariantValues { get; set; }

        public List<ShoppingCartLineTextOption> ShoppingCartLineTextOptions { get; set; }

        public Guid ShoppingCartID { get; set; }
		public ShoppingCart ShoppingCart { get; set; }
	}
}
