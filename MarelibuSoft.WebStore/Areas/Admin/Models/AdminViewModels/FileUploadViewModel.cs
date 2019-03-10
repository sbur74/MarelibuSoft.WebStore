using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
	public class FileUploadViewModel
	{
		public string RedirectUrl { get; set; }
		public string UploadPath { get; set; }
		public IFormFileCollection FormFiles{ get; set; }
	}
}
