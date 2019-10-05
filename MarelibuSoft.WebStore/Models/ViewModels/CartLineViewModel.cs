using System;
using System.Collections.Generic;

namespace MarelibuSoft.WebStore.Models.ViewModels
{
	public class CartLineViewModel
	{
		public int ID { get; set; }
		public int Position { get; set; }
		public int ProductID { get; set; }
        public int SellActionItemId { get; set; }
        public string ProductName { get; set; }
		public string ProductNo { get; set; }
		public string ShortDescription { get; set; }
		public decimal Quantity { get; set; }
		public string Unit { get; set; }
		public int UnitID { get; set; }
		public string ImgPath { get; set; }
		public decimal PosPrice { get; set; }
		public Guid CartID { get; set; }
		public decimal MinimumPurchaseQuantity { get; set; }
		public decimal AvailableQuantity { get; set; }
		public Guid ShoppingCartID { get; set; }
		public decimal SellBasePrice { get; set; }
		public decimal SellSekPrice { get; set; }
		public string SekUnit { get; set; }

		public string SlugUrl{ get; set; }

        public List<VariantViewModel> VariantList { get; set; }
        public List<TextOptionViewModel> TextOptionList { get; set; }
    }
}