namespace FuneralFlower_BE.Models
{
    public class News
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? TitleImageUrl { get; set; }
        public string? NewsContent { get; set; }
        public long CreateTime { get; set; }
    }
}
