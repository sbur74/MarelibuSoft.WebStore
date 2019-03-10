using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Enums;

namespace MarelibuSoft.WebStore.Models
{
	public class CmsPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[Display(Name = "Titel")]
		public string Titel { get; set; }
		[Display(Name = "Beschreibung (optional)")]
		public string PageDesciption { get; set; }	
		[Display(Name = "Seiteninhalt")]
		public string PageContent { get; set; }
		[Display(Name = "CMS Seite für")]
		public CmsPageEnum PageEnum { get; set; }
		[Display(Name = "Seiten Layout")]
		public CmsPageLayoutEnum LayoutEnum { get; set; }
		[Display(Name = "Bearbeitungsstatus")]
		public CmsPageStatusEnum StatusEnum { get; set; }
		[Display(Name = "Letzte Änderung")]
		public DateTime LastChange { get; set; }
		[Display(Name = "SEO Beschreibung")]
		public string SeoDescription { get; set; }
		[Display(Name = "SEO Suchwörter")]
		public string SeoKeywords { get; set; }
	}
}
