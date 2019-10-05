namespace MarelibuSoft.WebStore.Models
{
    public class VariantOptionTemplate
    {
        public int ID { get; set; }
        public string Option { get; set; }

        public int VariantTemplateId { get; set; }
        public VariantTemplate VariantTemplate { get; set; }
    }
}