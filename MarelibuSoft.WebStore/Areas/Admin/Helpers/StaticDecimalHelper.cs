using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Helpers
{
    public static class StaticDecimalHelper
    {
		public static decimal PaseString(string value)
		{
			CultureInfo deDE = new CultureInfo("de-DE");
			decimal result = 0.0m;

			if (!String.IsNullOrEmpty(value))
			{
				result = decimal.Parse(value, deDE.NumberFormat);
			}

			return result;
		}
    }
}
