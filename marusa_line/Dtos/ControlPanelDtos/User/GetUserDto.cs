namespace marusa_line.Dtos.ControlPanelDtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public string Location { get; set; }    
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int PaidOrdersCount { get; set; }
        public int UnPaidOrdersCount { get; set; }
        public int totalCount { get; set; }
    }
}
