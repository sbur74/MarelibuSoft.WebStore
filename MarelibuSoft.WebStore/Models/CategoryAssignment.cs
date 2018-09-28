using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class CategoryAssignment
    {
		public int ID { get; set; }
		public int ProductID { get; set; }
		public int CategoryID{ get; set; }
		public int CategorySubID { get; set; }
		public int CategoryDetailID { get; set; }

		public Product Product { get; set; }
	}
}
