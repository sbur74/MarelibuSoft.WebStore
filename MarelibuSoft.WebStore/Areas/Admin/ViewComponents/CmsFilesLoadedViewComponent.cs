using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.ViewComponents
{
	public class CmsFilesLoadedViewComponent : ViewComponent
	{
		private IWebHostEnvironment _environment;
		private string _uploadPath;
		private string _shopimages;

		public CmsFilesLoadedViewComponent(IWebHostEnvironment hostingEnvironment)
		{
			_environment = hostingEnvironment;
			_uploadPath = "contents";
			_shopimages = "images/store";
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			string path = Path.Combine(_environment.WebRootPath, _uploadPath);
			string products = Path.Combine(_environment.WebRootPath, _shopimages);


			var dir = new DirectoryInfo(path);
			var files = dir.GetFiles();
			var myfiles = new List<string>();
			//var proddir = new DirectoryInfo(products);
			//var prodfiles = proddir.GetFiles();

			foreach (var file in files)
			{
				myfiles.Add($"/{_uploadPath}/{file.Name}");
			}
			////foreach (var product in prodfiles)
			////{
			////	myfiles.Add($"/{_shopimages}/{product.Name}");
			////}
			return View(myfiles);
		}
	}
}
