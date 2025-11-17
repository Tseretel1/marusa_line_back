using marusa_line.Models;
using marusa_line.Dtos;
using marusa_line.Dtos.ControlPanelDtos;

namespace marusa_line.interfaces
{
    public interface ProductInterface
    {
        Task<List<Post>> GetPostsAsync(int productTypeId, int? userid);
        Task<List<Post>> GetUserLikedPosts(int userid);
        Task<List<GetOrdersDto>> GetUserOrders(int userId);
        Task<OrderDetailsDto> GetOrderById(int userId);
        Task<List<Photos>> GetAllPhotos();
        Task<bool> likeProduct(int userid, int productId);
        Task<List<Post>> GetMostDiscountedPosts(int? userid);
        Task<List<Post>> GetMostSoldProducts(int? userid);
        Task<Post> GetPostWithId(int id, int? userid);
        Task<Post?> GetOrderProduct(int id, int? userId = null);

        Task<List<ProductTypes>> GetProductTypes();
        Task<List<OrderStatuses>> GetOrderStatuses();
        Task<int> InsertOrderAsync(InsertOrder order);

        Task<UserOptionalFields> GetUser(int userid);
        Task<int> InsertLocation(int userId, string location);
        Task<int> InsertPhoneNumber(int userId, string phoneNumber);
    }
}
