using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Helpers
{
    public class UploadHelper 
    {
		private IWebHostEnvironment _environment;

		public UploadHelper(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		public async Task<FileNames> FileUploadAsync(IFormFile file, string targetpath, bool useguidname)
		{
			FileNames names = new FileNames();

			try
			{
				var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
				string fileName = string.Empty;
				string FilePath = parsedContentDisposition.FileName.ToString();
				var split = FilePath.Split('\\');
				string name = split.Last().Substring(0, split.Last().IndexOf("."));
				name = name.Replace("\"", "");
				string FileExtension = Path.GetExtension(FilePath);
				FileExtension = FileExtension.Replace("\"", "");
				string path = Path.Combine(_environment.WebRootPath, targetpath);

				if (!useguidname)
				{
					
					fileName = name + FileExtension;
				}
				else
				{
					string guidName = (Guid.NewGuid().ToString() + FileExtension);
					guidName = guidName.Replace("\\", "");
					guidName = guidName.Replace("\"", "");
					fileName = guidName;
				}

				fileName = fileName.Replace("\\", "");
				fileName = fileName.Replace("\"", "");
				using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
					names.Filename = fileName;
					names.Name = name;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return names;
		}
		public bool DeleteFile(string path, string filename)
		{
			try
			{
				if (!string.IsNullOrEmpty(path)&& !string.IsNullOrEmpty(filename))
				{
					string pathname = Path.Combine(_environment.WebRootPath, path);
					string fullname = Path.Combine(path, filename);
					FileInfo fileInfo = new FileInfo(fullname);
					if (fileInfo.Exists)
					{
						fileInfo.Delete();
						return fileInfo.Exists;
					} 
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			return false;
		}
    }
}
