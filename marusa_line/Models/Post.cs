namespace marusa_line.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string? PhotoUrl { get; set; }
        public int? PhotoId { get; set; }
        public int PostId { get; set; }
        public int LikeCount { get; set; }
        public List<Photos> Photos { get; set; } = new List<Photos>();

    }
}
