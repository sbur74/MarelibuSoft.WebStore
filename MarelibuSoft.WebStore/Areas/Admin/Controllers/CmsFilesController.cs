using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarelibuSoft.WebStore.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CmsFilesController : Controller
    {
		private IHostingEnvironment _environment;
		private string _uploadPath;
		public CmsFilesController(IHostingEnvironment hostingEnvironment)
		{
			_environment = hostingEnvironment;
			_uploadPath = "contents";
		}

		public IActionResult Index()
		{
			//string path = Path.Combine(_environment.WebRootPath, _uploadPath);

			//var dir = new DirectoryInfo(path);
			//var files = dir.GetFiles();
			//var myfiles = new List<string>();

			//foreach (var file in files)
			//{
			//	myfiles.Add($"/{_uploadPath}/{file.Name}");
			//}
			//return View(myfiles);
			return View();
		}

		[HttpPost("UploadFiles")]
		public async Task<IActionResult> Post(IFormCollection upload)
		{
			
			// full path to file in temp location
			var filePath = Path.GetTempFileName();
			var files = upload.Files;

			if (files == null)
			{
				files = HttpContext.Request.Form.Files;
			}

			long size = files.Sum(f => f.Length);

			var uploadhelper = new UploadHelper(_environment);

			foreach (var formFile in files)
			{
				if (formFile.Length > 0)
				{
					await uploadhelper.FileUploadAsync(formFile, _uploadPath, false);
				}
			}
			return RedirectToAction(nameof(Index));
		}
	}
}