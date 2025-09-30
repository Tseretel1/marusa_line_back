using marusa_line.Models;
using marusa_line.Dtos;

namespace marusa_line.interfaces
{
    public interface ProductInterface
    {
        Task<List<Post>> GetPostsAsync(int productTypeId, int? userid);
        Task<List<Post>> GetUserLikedPosts(int userid);
        Task<List<GetOrdersDto>> GetUserOrders(int userId);
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likeProduct(int userid, int productId);
        Task<List<Post>> GetMostDiscountedPosts(int? userid);
        Task<List<Post>> GetPostWithId(int id, int? userid);
        Task<int> InsertPostAsync(InsertPostDto dto);
        Task<int> EditPostAsync(InsertPostDto dto);
        Task<DateTime> deletePhoto(int photoId);
        Task<List<ProductTypes>> GetProductTypes();
        Task<List<OrderStatuses>> GetOrderStatuses();
        Task<int> InsertOrderAsync(int userId, int productId);
    }
}
