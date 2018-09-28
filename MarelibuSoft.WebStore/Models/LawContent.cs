using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class LawContent
    {
		public int ID { get; set; }
		public int SiteType { get; set; }
		public string Titel { get; set; }
		[DataType(DataType.MultilineText)]
		public string HtmlContent { get; set; }
	}
}
