using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Guest
    {
		public int GuestID { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string AdditionalAddress { get; set; }
		public string City { get; set; }
		public string PostCode { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
