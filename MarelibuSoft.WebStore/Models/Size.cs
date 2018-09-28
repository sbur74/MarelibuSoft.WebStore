using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Size
    {
		public int SizeID { get; set; }
		/// <summary>
		/// SIZE: XL,L,M,S,XS
		/// </summary>
		public string Name { get; set; }
		public string Description { get; set; }

	}
}
