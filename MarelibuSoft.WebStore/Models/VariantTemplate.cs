using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class VariantTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string OptionName { get; set; }

        public List<VariantOptionTemplate> VariantOptionTemplates { get; set; }
    }
}
