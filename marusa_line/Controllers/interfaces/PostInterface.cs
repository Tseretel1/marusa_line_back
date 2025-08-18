using marusa_line.Controllers.Models;

namespace marusa_line.Controllers.interfaces
{
    public interface PostInterface
    {
        Task<List<Post>> GetPostsAsync();
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likePost(int postId,string emoji);
    }
}
