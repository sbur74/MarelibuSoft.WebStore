using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
	public enum CmsPageEnum
	{
		Home,
		Faq,
		[Display(Name = "Impressum")]
		Imprint,
		[Display(Name = "Zahlung und Versand")]
		PaymendAndShipping,
		[Display(Name = "Im Netz")]
		OnWeb
	}
}
