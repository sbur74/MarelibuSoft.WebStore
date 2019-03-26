using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class OrderStateViewModel
    {
		public Guid ID { get; set; }
		public string StateAction { get; set; }
		public string TrackingNumber { get; set; }
		public string Message { get; set; }
	}
}
