using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class SellAction
    {
        public int SellActionID { get; set; }
        public string ActionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Percent { get; set; }
        public bool IsActive { get; set; }

        public List<SellActionItem> SellActionItems { get; set; }
    }
}
