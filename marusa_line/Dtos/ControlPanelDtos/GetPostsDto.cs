namespace marusa_line.Dtos.ControlPanelDtos
{
    public class GetPostsDto
    {
        public int? ProductTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public int PageNumber{ get; set; }
        public int PageSize{ get; set; }
    }
}
