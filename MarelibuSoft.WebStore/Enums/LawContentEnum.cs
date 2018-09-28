using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
    public enum LawContentEnum
    {
		[Display(Name = "nicht zugwiesen")]
		None = 0,
		[Display(Name = "Allgemeine Geschäftsbedingungen")]
		TAC = 1,
		[Display(Name = "Wiederrufsbelehrung")]
		CAL,
		[Display(Name = "Datenschutzerklärung")]
		PPO,
		[Display(Name ="Impressum")]
		IMP
	}
}
