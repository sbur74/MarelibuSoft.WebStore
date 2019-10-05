using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class ProductVariantValueViewModel
    {
        public int Id { get; set; }
        [Display(Name="Wert 1")]
        public string Value1 { get; set; }
        [Display(Name = "Wert 2")]
        public string Value2 { get; set; }
        [Display(Name = "Verfügbare Menge")]
        public double Quantity { get; set; }
        [Display(Name = "Preis")]
        public double Price { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }
    }
}
