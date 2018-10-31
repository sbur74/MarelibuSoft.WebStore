using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class NavItemViewModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int PerentId { get; set; }
		public string SlugUrl { get; set; }
		public List<NavItemViewModel> Childs { get; set; }
	}
}
