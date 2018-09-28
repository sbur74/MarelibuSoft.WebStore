using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Enums
{
    public enum PaymendTypeEnum
    {
		[Display(Name = "nicht zugewiesen")]
		None = 0,
		[Display(Name = "Überweisung")]
		Prepay,
		[Display(Name = "PayPal")]
		PayPal,
		[Display(Name = "Rechnung")]
		Bill
    }
}
