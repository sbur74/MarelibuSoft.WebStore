using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
	public enum CmsPageStatusEnum
	{
		[Display(Name ="In Bearbeitung")]
		Draft,
		[Display(Name = "Veröffentlicht")]
        Published,
        [Display(Name = "Alte Version")]
        Archived
	}
}
