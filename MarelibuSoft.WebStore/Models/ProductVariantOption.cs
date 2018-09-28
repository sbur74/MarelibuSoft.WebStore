namespace MarelibuSoft.WebStore.Models
{
	public class ProductVariantOption
	{
		public int ID { get; set; }
		public string Option { get; set; }

		public int ProductVariantID { get; set; }
		public ProductVariant Variant { get; set; }
	}
}