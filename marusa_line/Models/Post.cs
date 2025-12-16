namespace marusa_line.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int ProductTypeId { get; set; }
        public int LikeCount { get; set; } 
        public bool IsLiked { get; set; }
        public int quantity { get; set; }
        public string dateDeleted { get; set; }
        public int TotalActiveProducts { get; set; }
        public int TotalDeletedProducts { get; set; }
        public string? photoUrl { get; set; }
        public int totalCount { get; set; }
        public bool OrderNotAllowed { get; set; }

        public List<Photos> Photos { get; set; } = new List<Photos>();
    }
}
