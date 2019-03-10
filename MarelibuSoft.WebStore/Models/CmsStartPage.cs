using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
	public class CmsStartPage
	{
		public int ID { get; set; }
		[Display(Name = "Titel")]
		public string Title { get; set; }
		[Display(Name = "Begrüssungstext")]
		public string HeadContent { get; set; }
		[Display(Name = "Links")]
		public string LeftContent { get; set; }
		[Display(Name = "Rechts")]
		public string RightContent { get; set; }
		[Display(Name = "SEO Schlüsselwörter")]
		public string SeoKeywords { get; set; }
		[Display(Name = "SEO Beschreibung")]
		public string SeoDescription { get; set; }
	}
}
