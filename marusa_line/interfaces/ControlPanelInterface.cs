using Azure.Identity;
using marusa_line.Dtos;
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
        Task<Post?> GetPostWithIdControlPanel(int id, int? userId = null);
        Task<int> InsertPostAsync(InsertPostDto dto);
        Task<int> EditPostAsync(InsertPostDto dto);
        Task<int> RemoveProductById(int postId);
        Task<int> RevertProductById(int postId);

        Task<DateTime> deletePhoto(int photoId);
        Task<int> GetLikeCount();

    }
}
