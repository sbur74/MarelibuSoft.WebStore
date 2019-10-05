namespace MarelibuSoft.WebStore.Models
{
	public class ProductVariantOption
	{
		public int ID { get; set; }
		public string Option { get; set; }
        public string Combi { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsNotShown { get; set; }

        public int ProductVariantID { get; set; }
		public ProductVariant Variant { get; set; }
	}
}