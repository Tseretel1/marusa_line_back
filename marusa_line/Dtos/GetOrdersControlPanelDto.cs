namespace marusa_line.Dtos
{
    public class GetOrdersControlPanelDto
    {
        public int? UserId { get; set; }
        public int? OrderId { get; set; }
        public bool? IsPaid { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
