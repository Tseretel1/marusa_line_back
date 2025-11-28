using Dapper;
using System.Data;
using marusa_line.interfaces;
using Microsoft.Data.SqlClient;
using marusa_line.Dtos.ControlPanelDtos;
using marusa_line.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using marusa_line.Models;
using marusa_line.Dtos.ControlPanelDtos.Dashboard;
using marusa_line.Dtos.ControlPanelDtos.User;

namespace marusa_line.services
{
    public class ControlPanelService : ControlPanelInterface
    {
       
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public ControlPanelService(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("marusa_line_connection");
        }

        public async Task<List<OrderControlPanel>> GetOrdersControlPanel(GetOrdersControlPanelDto order)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, OrderControlPanel>();

            var result = await conn.QueryAsync<OrderControlPanel, Photos, User, OrderControlPanel>(
                "[dbo].[GetAllOrdersControlPanel]",
                (orderControl, photo, user) =>
                {
                    if (!lookup.TryGetValue(orderControl.OrderId, out var existingOrder))
                    {
                        existingOrder = orderControl;
                        existingOrder.Photos = new List<Photos>();
                        existingOrder.User = user;
                        lookup.Add(existingOrder.OrderId, existingOrder);
                    }

                    if (photo != null && photo.PhotoId != 0)
                    {
                        existingOrder.Photos.Add(photo);
                    }

                    return existingOrder;
                },
                param: new
                {
                    UserId = order.UserId,
                    IsPaid = order.IsPaid,
                    OrderId = order.OrderId,
                    PageNumber = order.PageNumber,
                    PageSize = order.PageSize,
                },
                splitOn: "PhotoId,UserId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }

        public async Task<List<Post>> GetPostsForAdminPanel(GetPostsDto getPosts)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<Post>(
                "[dbo].[GetProductsForControlPanel]",
                new
                {
                    IsDeleted = getPosts.IsDeleted,
                    PageNumber = getPosts.PageNumber,
                    PageSize = getPosts.PageSize
                },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }


        public async Task<int> ToggleOrderIsPaidAsync(int orderId, bool isPaid,int quantity)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            parameters.Add("@Paid", isPaid, DbType.Boolean);
            parameters.Add("@OrderCount", quantity, DbType.Int32);

            var rowsAffected = await conn.ExecuteAsync(
                "[dbo].[ChangeOrderIsPaid]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
        public async Task<int> ChangeOrderStatus(int orderId, int isPaid)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            parameters.Add("@StatusId", isPaid, DbType.Int32);

            var rowsAffected = await conn.ExecuteAsync(
                "[dbo].[ChangeOrderStatus]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
        public async Task<int> GetOrdersTotalCountAsync(bool? isPaid)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@Paid", isPaid, DbType.Boolean);

            var totalCount = await conn.QuerySingleAsync<int>(
                "[dbo].[getOrdersTotalCount]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return totalCount;
        }

        public async Task<int> DeleteOrder(int orderId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            var rowsAffected = await conn.ExecuteAsync(
                "[dbo].[DeleteOrder]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }

        public async Task<ControlPanelLoginReturn> Login(ControlPanelLoginDto loginDto)
        {
            if(loginDto.Username =="giorgi"&& loginDto.Password == "giorgi")
            {
                var jwtToken = CreateJwtForUser(loginDto.Username);
                var ControlPanelReturn = new ControlPanelLoginReturn
                {
                    Token = jwtToken,
                };
                return ControlPanelReturn;
            }
            return null;
        }


        private string CreateJwtForUser(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("username", username),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Post?> GetPostWithIdControlPanel(int id, int? userId = null)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetProductsByIdForControlPanel]",
                (post, photo) =>
                {
                    if (!lookup.TryGetValue(post.Id, out var existingPost))
                    {
                        existingPost = post;
                        existingPost.Photos = new List<Photos>();
                        lookup.Add(existingPost.Id, existingPost);
                    }

                    if (photo != null && photo.PhotoId > 0)
                    {
                        existingPost.Photos.Add(photo);
                    }

                    return existingPost;
                },
                param: new
                {
                    Id = id,
                    UserId = userId
                },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );
            return lookup.Values.FirstOrDefault();
        }
        public async Task<int> InsertPostAsync(InsertPostDto dto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var postId = await conn.ExecuteScalarAsync<int>(
                "[dbo].[InsertProduct]",
                new
                {
                    dto.Title,
                    dto.Description,
                    dto.Price,
                    dto.DiscountedPrice,
                    dto.Quantity,
                    dto.ProductTypeId
                },
                commandType: CommandType.StoredProcedure
            );
            foreach (var photo in dto.Photos)
            {
                await conn.ExecuteAsync(
                    "[dbo].[InsertPhoto]",
                    new { ProductId = postId, photo.PhotoUrl },
                    commandType: CommandType.StoredProcedure
                );
            }
            return postId;
        }

        public async Task<int> EditPostAsync(InsertPostDto dto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var postId = await conn.ExecuteScalarAsync<int>(
                "[dbo].[EditProduct]",
                new
                {
                    dto.Id,
                    dto.Title,
                    dto.Description,
                    dto.Price,
                    dto.DiscountedPrice,
                    dto.Quantity,
                    dto.ProductTypeId
                },
                commandType: CommandType.StoredProcedure
            );
            foreach (var photo in dto.Photos)
            {
                await conn.ExecuteAsync(
                    "[dbo].[InsertPhoto]",
                    new { ProductId = postId, photo.PhotoUrl },
                    commandType: CommandType.StoredProcedure
                );
            }
            return postId;
        }

        public async Task<int> RemoveProductById(int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", productId, DbType.Int32);
            var rowsAffected = await conn.QuerySingleAsync<int>(
                "[dbo].[RemoveProductById]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
        public async Task<int> RevertProductById(int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", productId, DbType.Int32);
            var rowsAffected = await conn.QuerySingleAsync<int>(
                "[dbo].[RevertProductById]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }

        public async Task<DateTime> deletePhoto(int photoId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var photoDeleted = await conn.ExecuteScalarAsync<DateTime>(
                "[dbo].[DeletePhoto]",
                new
                {
                    PhotoId = photoId,
                },
                commandType: CommandType.StoredProcedure
            );
            return photoDeleted;
        }

        public async Task<int> GetLikeCount()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var count = await conn.QuerySingleAsync<int>(
                "[dbo].[GetLikesCount]",
                commandType: CommandType.StoredProcedure
            );
            return count;
        }

        public async Task<DashboardStats> GetDashboardStatistics(GetDahsboard stats)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QuerySingleAsync<DashboardStats>(
                "[dbo].[GetOrderStatistics]",
                new
                {
                    StartDate = stats.StartDate,
                    EndDate = stats.EndDate
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
        public async Task<DashboardStatsByYear> GetDashboard(int year)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var statsByMonth = (await conn.QueryAsync<DashboardStatsByMonths>(
                "[dbo].[spGetMonthlyOrderStats]",
                new { Year = year },
                commandType: CommandType.StoredProcedure
            )).ToList();

            var YearStat = await conn.QuerySingleAsync<DashboardYearSum>(
                "[dbo].[spGetYearlyOrderStats]",
                new { Year = year },
                commandType: CommandType.StoredProcedure
            );

            return new DashboardStatsByYear
            {
                statsByMonth = statsByMonth,
                YearStat = YearStat
            };
        }

        public async Task<List<SoldProductTypes>> GetSoldProductTypes(int year, int? month)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<SoldProductTypes>(
                "[dbo].[spGetProductTypeSalesStats]",
                new
                {
                    Year = year,
                    Month= month
                },
                commandType: CommandType.StoredProcedure
            );
            return result.ToList();
        }
  

        public async Task<List<ProductTypes>> InsertProducType(string productType)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<ProductTypes>(
                "[dbo].[AddProductType]",
                new
                {
                    productType = productType,
                },
                commandType: CommandType.StoredProcedure
            );
            return result.ToList();
        }

        public async Task<List<ProductTypes>> EditProductType(int id, string productType)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<ProductTypes>(
                "[dbo].[EditProductType]",
                new
                {
                    Id = id,
                    productType = productType,
                },
                commandType: CommandType.StoredProcedure
            );
            return result.ToList();
        }
        public async Task<List<ProductTypes>> DeleteProductType(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<ProductTypes>(
                "[dbo].[DeleteProductType]",
                new
                {
                    Id = id,
                },
                commandType: CommandType.StoredProcedure
            );
            return result.ToList();
        }

        public async Task<GetUserDto> GetUser(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QuerySingleAsync<GetUserDto>(
                "[dbo].[GetuserById]",
                new
                {
                    Id = id,
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<List<GetUserDto>> GetUsersList(GetUserFilteredDto dto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var multi = await conn.QueryMultipleAsync(
                "[dbo].[GetUsersList]",
                new
                {
                    UserId = dto.UserId,
                    IsBlocked = dto.IsBlocked,
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

        public async Task<int> UpdateUserRole(int userId, string role)
        {
            using var conn = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            parameters.Add("Role", role);
            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await conn.ExecuteAsync(
                "[dbo].[UpdateRole]",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return parameters.Get<int>("ReturnValue");
        }

    }
}
