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
        public List<Photos> Photos { get; set; } = new List<Photos>();

    }
}
