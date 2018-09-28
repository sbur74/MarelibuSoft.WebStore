using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
	public class CustomerIndexOrderViewModel
	{
		public Guid ID { get; set; }
		[Display(Name = "Auftragsnummer")]
		public string Number { get; set; }
		[Display(Name = "Auftragsdatum")]
		public string Orderdate { get; set; }
		[Display(Name ="Auftragstatus")]
		public string OrderState { get; set; }
	}
}
