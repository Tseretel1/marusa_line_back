using marusa_line.Models;

namespace marusa_line.interfaces
{
    public interface PostInterface
    {
        Task<List<Post>> GetPostsAsync();
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likePost(int postId,string emoji);
    }
}
