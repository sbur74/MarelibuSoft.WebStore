using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
	public enum CmsPageLayoutEnum
	{
		[Display(Name = "Einspaltig")]
		OneColumn,
		[Display(Name = "Zweispaltig")]
		TowColumn,
		[Display(Name = "Dreispaltig")]
		TreeColumn
	}
}
