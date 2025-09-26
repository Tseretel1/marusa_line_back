namespace marusa_line.Dtos
{
    public class AuthResultDto
    {
        public string Token { get; set; } = "";
        public UserDto User { get; set; } = new UserDto();
    }
}
