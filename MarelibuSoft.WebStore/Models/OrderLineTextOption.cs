using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class OrderLineTextOption
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public string Name { get; set; }

        public int OrderLineId { get; set; }
        public OrderLine OrderLine { get; set; }
    }
}
