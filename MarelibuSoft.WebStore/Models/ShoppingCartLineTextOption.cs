using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class ShoppingCartLineTextOption
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public int ShoppingCartLineId { get; set; }
        public ShoppingCartLine ShoppingCartLine { get; set; }
    }
}
