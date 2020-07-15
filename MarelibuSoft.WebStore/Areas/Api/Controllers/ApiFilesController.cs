using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiFilesController : ControllerBase
    {
		private IWebHostEnvironment _environment;

		public ApiFilesController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		// GET: api/ApiFiles
		[HttpGet]
        public IEnumerable<string> Get()
        {
			string targetpath = "contents";
			string path = Path.Combine(_environment.WebRootPath, targetpath);

			var dir = new DirectoryInfo(path);
			var files = dir.GetFiles();
			var myfiles = new List<string>();

			foreach (var file in files)
			{
				myfiles.Add($"/{targetpath}/{file.Name}");
			}
			
			return myfiles;
        }

        // GET: api/ApiFiles/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ApiFiles
        [HttpPost]
		[Route("upload")]
		public IActionResult Post(IFormCollection fileUpload)
        {
			if (fileUpload != null)
			{
				var files = fileUpload.Files;



				return Ok();
			}
			else
			{
				return NoContent();
			}
        }

        // PUT: api/ApiFiles/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
