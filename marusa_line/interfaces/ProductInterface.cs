using marusa_line.Models;
using marusa_line.Dtos;

namespace marusa_line.interfaces
{
    public interface ProductInterface
    {
        Task<List<Post>> GetPostsAsync(int productTypeId, int? userid);
        Task<List<Post>> GetPostsForAdminPanel(int productTypeId, int? userId);
        Task<List<Post>> GetUserLikedPosts(int userid);
        Task<List<GetOrdersDto>> GetUserOrders(int userId);
        Task<List<OrderControlPanel>> GetOrdersControlPanel(GetOrdersControlPanelDto order);
        Task<OrderDetailsDto> GetOrderById(int userId);
        
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likeProduct(int userid, int productId);
        Task<List<Post>> GetMostDiscountedPosts(int? userid);
        Task<List<Post>> GetMostSoldProducts(int? userid);
        Task<Post> GetPostWithId(int id, int? userid);
        Task<Post?> GetOrderProduct(int id, int? userId = null);
        Task<Post> GetPostWithIdControlPanel(int id, int? userid);
        Task<int> InsertPostAsync(InsertPostDto dto);
        Task<int> EditPostAsync(InsertPostDto dto);
        Task<int> RemoveProductById(int postId);
        Task<int> RevertProductById(int postId);

        Task<DateTime> deletePhoto(int photoId);
        Task<List<ProductTypes>> GetProductTypes();
        Task<List<OrderStatuses>> GetOrderStatuses();
        Task<int> InsertOrderAsync(InsertOrder order);

        Task<UserOptionalFields> GetUser(int userid);
        Task<int> InsertLocation(int userId, string location);
        Task<int> GetLikeCount();
        Task<int> InsertPhoneNumber(int userId, string phoneNumber);
    }
}
