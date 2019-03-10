using MarelibuSoft.WebStore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
	public class CmsPageWorkViewModel
	{
		public int Id { get; set; }
		public int DraftOfPageId { get; set; }
		public string Name { get; set; }
		[Display(Name = "Titel")]
		public string Titel { get; set; }
		[Display(Name = "Beschreibung (optional)")]
		public string PageDesciption { get; set; }
		[Display(Name = "Seiteninhalt")]
		public string PageContent { get; set; }
		[Display(Name = "Seiteninhalt-Links")]
		public string PageContentLeft { get; set; }
		[Display(Name = "Seiteninhalt-Rechts")]
		public string PageContentRight { get; set; }
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
