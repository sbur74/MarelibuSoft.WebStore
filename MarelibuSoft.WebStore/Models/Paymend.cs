using MarelibuSoft.WebStore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class Paymend
    {
		public int ID { get; set; }
		public string Name { get; set; }
		public string LogoUrl { get; set; }
		public bool IsActive { get; set; }
		public PaymendTypeEnum PaymendType { get; set; }
	}
}
