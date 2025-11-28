using Azure.Identity;
using marusa_line.Dtos;
using marusa_line.Dtos.ControlPanelDtos;
using marusa_line.Dtos.ControlPanelDtos.Dashboard;
using marusa_line.Dtos.ControlPanelDtos.User;
using marusa_line.Models;

namespace marusa_line.interfaces
{
    public interface ControlPanelInterface
    {
        Task<List<OrderControlPanel>> GetOrdersControlPanel(GetOrdersControlPanelDto order);
        Task<List<Post>> GetPostsForAdminPanel(GetPostsDto getPosts);
        Task<int> ToggleOrderIsPaidAsync(int orderId,bool isPaid, int quantity);
        Task<int> ChangeOrderStatus(int orderId, int isPaid);
        Task<int> DeleteOrder(int orderId);
        Task<int> GetOrdersTotalCountAsync(bool? isPaid);
        Task<ControlPanelLoginReturn> Login (ControlPanelLoginDto loginDto);
        Task<Post?> GetPostWithIdControlPanel(int id, int? userId = null);
        Task<int> InsertPostAsync(InsertPostDto dto);
        Task<int> EditPostAsync(InsertPostDto dto);
        Task<int> RemoveProductById(int postId);
        Task<int> RevertProductById(int postId);
        Task<DateTime> deletePhoto(int photoId);
        Task<int> GetLikeCount();
        Task<DashboardStats> GetDashboardStatistics(GetDahsboard stats);
        Task<DashboardStatsByYear> GetDashboard(int year);
        Task<List<SoldProductTypes>> GetSoldProductTypes(int year,int? month);
        Task<List<ProductTypes>> InsertProducType(string productType);
        Task<List<ProductTypes>> EditProductType(int id,string productType);
        Task<List<ProductTypes>> DeleteProductType(int id);
        Task<GetUserDto> GetUser(int id);
        Task<List<GetUserDto>>SearchUserByName(string search);
        Task<List<GetUserDto>> SearchUserByEmail(string search);

        Task<int> UpdateUserRole(int userId, string role);
        Task<List<GetUserDto>> GetUsersList(GetUserFilteredDto dto);
    }
}
