using Azure.Identity;
using marusa_line.Dtos.ControlPanelDtos;
using marusa_line.Models;

namespace marusa_line.interfaces
{
    public interface ControlPanelInterface
    {
        Task<List<Post>> GetPostsForAdminPanel(GetPostsDto getPosts);
        Task<int> ToggleOrderIsPaidAsync(int orderId,bool isPaid);
        Task<int> ChangeOrderStatus(int orderId, int isPaid);
        Task<int> DeleteOrder(int orderId);
        Task<int> GetOrdersTotalCountAsync(bool? isPaid);
        Task<ControlPanelLoginReturn> Login (ControlPanelLoginDto loginDto);
    }
}
