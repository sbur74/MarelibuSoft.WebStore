using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class WeHaveYourOrderLineViewModel
    {
		[Key]
		public int ID { get; set; }
		public string ImagePath { get; set; }
		public int Position { get; set; }
		public int ProductNumber { get; set; }
		public string ProductName { get; set; }
		public decimal Quantity { get; set; }
		public string ProductUnit { get; set; }
		public decimal Price { get; set; }

        public List<TextOptionViewModel> TextOptionsList { get; set; }
        public List<VariantViewModel> VariantList { get; set; }
    }
}
