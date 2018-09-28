using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class SellActionItem
    {
		public int SellActionItemID { get; set; }
		public int FkProductID { get; set; }

		public int SellActionID { get; set; }
		public SellAction SellAction { get; set; }
	}
}
