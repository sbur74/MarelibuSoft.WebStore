using System.Collections.Generic;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
    public class ProductVariantOptionViewModel
    {
        public int ID { get; set; }
        public string Option { get; set; }
        public Dictionary<int, string> Combi{ get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsNotShown { get; set; }

        public int ProductVariantID { get; set; }
    }
}