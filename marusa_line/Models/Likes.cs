namespace marusa_line.Controllers.Models
{
    public class Likes
    {
        public int id { get; set; }
        public int? LikeId{ get; set; }
        public int PostId { get; set; }
        public string Emoji{ get; set; }
    }
}
