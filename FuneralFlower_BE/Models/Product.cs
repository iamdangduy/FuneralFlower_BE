namespace FuneralFlower_BE.Models
{
    public class Product
    {
        public string? Id { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductOldPrice { get; set; }
        public decimal ProductNewPrice { get; set; }
        public string? Description { get; set; }
        public string? ProductImageUrl { get; set; }
        public string? ProductCategoryId { get; set; }
        public long CreateTime { get; set; }
    }
}
