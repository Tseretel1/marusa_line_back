namespace marusa_line.Dtos.ControlPanelDtos.User
{
    public class GetUserFilteredDto
    {
        public int? UserId { get; set; }       
        public bool? IsBlocked { get; set; }    
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
