namespace marusa_line.Dtos
{
    public class GetProductDto
    {
        public int userId { get; set; }
        public int? productTypeId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
