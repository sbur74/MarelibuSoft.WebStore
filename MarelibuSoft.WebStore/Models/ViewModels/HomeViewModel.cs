using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class HomeViewModel
    {
		public List<SliderViewModel> SliderViews { get; set; }
		public string NewsImage { get; set; }
		public string SectionTreeText { get; set; }
		public HomeViewModel()
		{
			SliderViews = new List<SliderViewModel>();
		}

		public CmsStartPage StartPage { get; set; }
	}
}
