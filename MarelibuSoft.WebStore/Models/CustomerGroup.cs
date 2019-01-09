using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class CustomerGroup
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Rabatt { get; set; }

		public List<CustomerGroupAssignment> Assignments { get; set; }
	}
}
