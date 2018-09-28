using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
	public class OrderEmailViewModel
	{
		public Guid OrderID { get; set; }
		[Display(Name ="EMail Adresse")]
		public string Email { get; set; }
		[Display(Name = "Nachricht")]
		public string Message { get; set; }
		[Display(Name = "Betreff")]
		public string Subject { get; set; }
		[Display(Name = "Anhang")]
		public List<IFormFile> Attachments { get; set; }
	}
}
