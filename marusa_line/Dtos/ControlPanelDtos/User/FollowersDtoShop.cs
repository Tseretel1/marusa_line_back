namespace marusa_line.Dtos.ControlPanelDtos.User
{
    public class FollowersDtoShop
    {
         public IEnumerable<UserDto> Users { get; set; }
         public int TotalUsers { get; set; }
         public int NonBlockedUsers { get; set; }
         public int BlockedUsers { get; set; }
    }
}
