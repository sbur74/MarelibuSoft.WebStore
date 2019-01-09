using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class CustomerGroupAssignment
	{
		public int ID { get; set; }
		
		public Guid CustomerID { get; set; }

		public int GroupID { get; set; }
		public CustomerGroup Group { get; set; }
	}
}
