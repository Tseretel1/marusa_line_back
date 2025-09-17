using marusa_line.Models;
using marusa_line.Dtos;

namespace marusa_line.interfaces
{
    public interface PostInterface
    {
        Task<List<Post>> GetPostsAsync(int productTypeId);
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likePost(int postId,string emoji);
        Task<List<Post>> GetMostDiscountedPosts();
        Task<List<Post>> GetPostWithId(int id);
        Task<int> InsertPostAsync(InsertPostDto dto);
        Task<int> EditPostAsync(InsertPostDto dto);
        Task<DateTime> deletePhoto(int photoId);
        Task<List<ProductTypes>> GetProductTypes();

    }
}
