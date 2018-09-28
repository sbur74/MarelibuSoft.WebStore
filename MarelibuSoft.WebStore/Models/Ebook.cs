using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class Ebook
	{
		public int EbookID{ get; set; }
		public string Path { get; set; }
		public string PathFileID { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }

		public int FkProductID { get; set; }
	}
}
