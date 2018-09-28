using MarelibuSoft.WebStore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShopFile
    {
		public int ID { get; set; }
		public string Name { get; set; }
		public string Filename { get; set; }
		public ShopFileTypeEnum ShopFileType { get; set; }
		public bool IsActive { get; set; }
	}
}
