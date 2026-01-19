using marusa_line.Dtos.ControlPanelDtos.NewFolder;
using marusa_line.Dtos.ControlPanelDtos.ShopDtos;
using marusa_line.Dtos.AdminPanelDtos;
using marusa_line.Dtos.ControlPanelDtos.User;

namespace marusa_line.interfaces
{
    public interface AdminInterface
    {
        Task<ICollection<ShopDto>> GetEveryShop();
        Task<ShopDto> GetShopById(int shopId);
        Task<ShopStatsDto> GetShopStats(int shopId);
        Task<bool> UpdateShopAsync(ShopDto shop);
        Task<bool> UpdateSubscription(SubscriptionDto subscripion);
        Task<bool> AddShop(AddShop shop);
        Task<bool> UpdateShopPassword(UpdateShopPassword shop);
        Task<List<GetUserDto>> GetUsersList(Pagination dto);

    }
}
