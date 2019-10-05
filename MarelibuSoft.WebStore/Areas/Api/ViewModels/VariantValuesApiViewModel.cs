using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Api.ViewModels
{
    public class VariantValuesApiViewModel
    {
        public int ProductId { get; set; }
        public string Titel { get; set; }
        public IEnumerable<ProductVariantValue> Values{ get; set; }
    }
}
