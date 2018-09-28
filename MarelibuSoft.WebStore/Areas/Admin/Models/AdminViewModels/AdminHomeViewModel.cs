using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class AdminHomeViewModel
    {
		[Display( Name = "Anzahl offener Aufträge")]
		public int OpenOrdersCount { get; set; }
		[Display(Name = "Anzahl offener unbezahlter Aufträge")]
		public int OpenNotPayedOrdersCount { get; set; }
		[Display(Name = "Anzahl offener versendeter Aufträge")]
		public int OpenSendOrdersCount { get; set; }
		[Display(Name = "Anzahl abgeschlossene Aufträge")]
		public int ClosedOrdersCount { get; set; }
	}
}
