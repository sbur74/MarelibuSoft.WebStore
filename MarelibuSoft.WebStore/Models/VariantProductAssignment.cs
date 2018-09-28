using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class VariantProductAssignment
	{
		public int ID { get; set; }
		public int VariantID { get; set; }
		public int ProductID { get; set; }
	}
}
