using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
	public class CustomerOrderViewModel
	{
		public Guid ID { get; set; }
		[Display(Name = "Auftragsnummer")]
		public string Number { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
		public DateTime OrderDate { get; set; }
		[Display(Name = "E-Mail Adresse")]
		public string EMail { get; set; }
		[Display(Name = "Kundenwunsch")]
		public string FreeText { get; set; }
		[Display(Name = "Rechung bezahlt")]
		public bool IsPayed { get; set; }
		[Display(Name = "Ware versendet")]
		public bool IsSend { get; set; }
		[Display(Name = "Auftrag abgeschlossen")]
		public bool IsClosed { get; set; }
	}
}
