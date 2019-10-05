using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class ProductVariantViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string OptionName { get; set; }
        public bool IsAbsolutelyNecessary { get; set; }
        public List<ProductVariantOptionViewModel> Options { get; set; }
        public int ProductId { get; set; }
    }
}
