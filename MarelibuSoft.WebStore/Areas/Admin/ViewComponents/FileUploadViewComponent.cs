using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.ViewComponents
{
	public class FileUploadViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(FileUploadViewModel model)
		{
			return View(model);
		}
	}
}
