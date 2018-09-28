using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
    public enum ShopFileTypeEnum
    {
		[Display(Name = "nicht zugwiesen")]
		None = 0,
		[Display(Name = "Allgemeine Geschätsbedingungen")]
		AGB = 1,
		[Display(Name ="Wiederrufsbelehrung")]
		WRB,
		[Display(Name = "Datenschutzerklärung")]
		DSK
    }
}
