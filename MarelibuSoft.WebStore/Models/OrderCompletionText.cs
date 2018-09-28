using MarelibuSoft.WebStore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models
{
    public class OrderCompletionText
    {
		public int ID { get; set; }
		[Display(Name="Zahlungsart")]
		public PaymendTypeEnum PaymendType { get; set; }
		public string Name { get; set; }
		[Display(Name = "HTML Text")]
		public string Content { get; set; }
	}
}
