using marusa_line.Controllers.Models;
using marusa_line.Controllers.NewFolder;

namespace marusa_line.Controllers.interfaces
{
    public interface PostInterface
    {
        Task<List<Post>> GetPostsAsync();
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likePost(int postId,string emoji);
        Task<List<Post>> GetMostDiscountedPosts();
        Task<List<Post>> GetPostWithId(int id);
        Task<int> InsertPostAsync(InsertPostDto dto);
        Task<int> EditPostAsync(InsertPostDto dto);
        Task<DateTime> deletePhoto(int photoId);
    }
}
