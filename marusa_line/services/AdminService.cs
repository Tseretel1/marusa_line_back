using marusa_line.Dtos.ControlPanelDtos.ShopDtos;
using marusa_line.interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using marusa_line.Dtos.ControlPanelDtos.NewFolder;
using marusa_line.Dtos.AdminPanelDtos;
using marusa_line.Dtos.ControlPanelDtos.User;

namespace marusa_line.services
{
    public class AdminService : AdminInterface
    {

        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public AdminService(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("marusa_line_connection");
        }

        public async Task<ICollection<ShopDto>> GetEveryShop()
        {
            using var conn = new SqlConnection(_connectionString);

            var result = await conn.QueryAsync<ShopDto>(
                "[dbo].[getEveryShop]",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
        public async Task<ShopDto?> GetShopById(int shopId)
        {
            using var conn = new SqlConnection(_connectionString);

            return await conn.QuerySingleOrDefaultAsync<ShopDto>(
                "[dbo].[spGetShopById]",
                new { ShopId = shopId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<ShopStatsDto> GetShopStats(int shopId)
        {
            using var conn = new SqlConnection(_connectionString);

            return await conn.QuerySingleAsync<ShopStatsDto>(
                "[dbo].[spGetShopStats]",
                new { ShopId = shopId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<bool> UpdateShopAsync(ShopDto shop)
        {
            using var conn = new SqlConnection(_connectionString);

            var result = await conn.QuerySingleAsync<bool>(
                "[dbo].[spUpdateShop]",
                new
                {
                    ShopId = shop.Id,
                    Name = shop.Name,
                    Logo = shop.Logo,
                    Location = shop.Location,
                    Gmail = shop.Gmail,
                    Subscription = shop.Subscription,
                    Instagram = shop.Instagram,
                    Facebook = shop.Facebook,
                    Titkok = shop.Tiktok,
                    BOG = shop.Bog,
                    TBC = shop.Tbc,
                    Receiver = shop.Receiver,
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<bool> UpdateSubscription(SubscriptionDto subscripion)
        {
            using var conn = new SqlConnection(_connectionString);

            var result = await conn.QuerySingleAsync<bool>(
                "[dbo].[UpdateSubscription]",
                new
                {
                    ShopId = subscripion.ShopId,
                    Subscription= subscripion.Subscription,
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<bool> AddShop(AddShop shop)
        {
            using var conn = new SqlConnection(_connectionString);

            var result = await conn.QuerySingleAsync<bool>(
                "[dbo].[AddShop]",
                new
                {
                    ShopName = shop.ShopName,
                    Gmail = shop.Gmail,
                    Password = shop.Password,
                },
                commandType: CommandType.StoredProcedure
            ) ;

            return result;
        }
        public async Task<bool> UpdateShopPassword(UpdateShopPassword shop)
        {
            using var conn = new SqlConnection(_connectionString);

            var result = await conn.QuerySingleAsync<bool>(
                "[dbo].[UpdateShopPassword]",
                new
                {
                    ShopId = shop.ShopId,
                    Password = shop.Password,
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<List<GetUserDto>> GetUsersList(Pagination dto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var multi = await conn.QueryMultipleAsync(
                "[dbo].[GetUsers]",
                new
                {
                    UserName = dto.UserName,
                    Gmail = dto.Gmail,
                    PageNumber = dto.PageNumber,
                    PageSize = dto.PageSize
                },
                commandType: CommandType.StoredProcedure
            );

            var users = (await multi.ReadAsync<GetUserDto>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();
            users[0].totalCount = totalCount;

            return users;
        }
    }
}
