using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShoppingCart
    {
		public Guid ID { get; set; }
		public string Number { get; set; }
		public Guid OrderId { get; set; }
		public Guid CustomerId { get; set; }
		public List<ShoppingCartLine> Lines { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime LastChange { get; set; }
		public int TabCounter { get; set; }
		public string SessionId { get; set; }
	}
}
