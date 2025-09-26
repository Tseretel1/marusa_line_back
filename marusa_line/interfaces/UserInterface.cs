using marusa_line.Dtos;
using marusa_line.Models;

namespace marusa_line.interfaces
{
    public interface UserInterface
    {
        Task<AuthResultDto?> GoogleCallbackAsync(string code, string state,string expectedState);
        string GetGoogleAuthUrl();
        Task<bool> InsertUserIfNotExistsAsync(User user);
    }
}
