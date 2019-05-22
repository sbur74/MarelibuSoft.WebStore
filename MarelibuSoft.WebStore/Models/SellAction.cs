using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class SellAction
    {
        public int SellActionID { get; set; }
        [Display(Name = "Aktion")]
        public string ActionName { get; set; }
        [Display(Name ="Startdatum")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Enddatum")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Rabat in %")]
        public decimal Percent { get; set; }
        [Display(Name = "aktiv")]
        public bool IsActive { get; set; }

        public List<SellActionItem> SellActionItems { get; set; }
    }
}
