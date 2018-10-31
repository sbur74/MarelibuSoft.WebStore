using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Category
    {
		public int ID { get; set; }
		public string Name { get; set; }
		[Display(Name = "SEO Beschreibung")]
		[StringLength(155,ErrorMessage = "Du darfst nicht mehr als 155 Zeichen vervenden!")]
		public string SeoDescription { get; set; }
		[Display(Name = "HTML Beschreibung")]
		public string HtmlDescription { get; set; }
		[Display(Name = "SEO Schlagworte(Keywords)")]
		public string SeoKeywords { get; set; }
		[Display(Name = "Unterkategorien")]
		public List<CategorySub> Subs { get; set; }
	}
}
