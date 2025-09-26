using marusa_line.Dtos;

namespace marusa_line.interfaces
{
    public interface UserInterface
    {
        Task<AuthResultDto?> GoogleCallbackAsync(string code, string state,string expectedState);
        string GetGoogleAuthUrl();
    }
}
