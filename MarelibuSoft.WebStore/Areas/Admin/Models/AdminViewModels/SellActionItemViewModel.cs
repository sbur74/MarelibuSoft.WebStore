using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels
{
    public class SellActionItemViewModel
    {
        public int SellActionItemID { get; set; }
        public int FkProductID { get; set; }
        public int SellActionID { get; set; }
        public string Name { get; set; }
        public int No { get; set; }
        public string Img { get; set; }
    }
}
