using marusa_line.interfaces;
using marusa_line.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using marusa_line.Dtos;

namespace marusa_line.services
{
    public class ProductService : ProductInterface
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("marusa_line_connection");
        }
        public async Task<List<Post>> GetPostsAsync(int productTypeId, int? userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetProducts]",
                (post, photo) =>
                {
                    if (!lookup.TryGetValue(post.Id, out var existingPost))
                    {
                        existingPost = post;
                        existingPost.Photos = new List<Photos>();
                        lookup.Add(existingPost.Id, existingPost);
                    }

                    if (photo != null && photo.PhotoId != null)
                    {
                        existingPost.Photos.Add(photo);
                    }

                    return existingPost;
                },
                param: new
                {
                    ProductId = productTypeId,
                    UserId = userId
                },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }
        public async Task<List<GetOrdersDto>> GetUserOrders(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, GetOrdersDto>(); 

            var result = await conn.QueryAsync<GetOrdersDto, Photos, GetOrdersDto>(
                "[dbo].[GetMyOrders]",
                (order, photo) =>
                {
                    if (!lookup.TryGetValue(order.OrderId, out var existingOrder))
                    {
                        existingOrder = order;
                        existingOrder.Photos = new List<Photos>();
                        lookup.Add(existingOrder.OrderId, existingOrder); 
                    }

                    if (photo != null && photo.PhotoId != null)
                    {
                        existingOrder.Photos.Add(photo);
                    }

                    return existingOrder;
                },
                param: new
                {
                    UserId = userId
                },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }


        public async Task<List<Post>> GetUserLikedPosts(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetLikedPostsByUser]",
                (post, photo) =>
                {
                    if (!lookup.TryGetValue(post.Id, out var existingPost))
                    {
                        existingPost = post;
                        existingPost.Photos = new List<Photos>();
                        lookup.Add(existingPost.Id, existingPost);
                    }

                    if (photo != null && photo.PhotoId != null)
                    {
                        existingPost.Photos.Add(photo);
                    }

                    return existingPost;
                },
                param: new
                {
                    UserId = userId
                },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }

        public async Task<List<Post>> GetMostDiscountedPosts(int? userId = null)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetTopDiscountedProducts]",
                (post, photo) =>
                {
                    if (!lookup.TryGetValue(post.Id, out var existingPost))
                    {
                        existingPost = post;
                        existingPost.Photos = new List<Photos>();
                        lookup.Add(existingPost.Id, existingPost);
                    }

                    if (photo != null && photo.PhotoId != null)
                    {
                        existingPost.Photos.Add(photo);
                    }

                    return existingPost;
                },
                param: new
                {
                    UserId = userId  
                },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }
        public async Task<List<Post>> GetMostSoldProducts(int? userId = null)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetTopSoldProducts]",
                (post, photo) =>
                {
                    if (!lookup.TryGetValue(post.Id, out var existingPost))
                    {
                        existingPost = post;
                        existingPost.Photos = new List<Photos>();
                        lookup.Add(existingPost.Id, existingPost);
                    }

                    if (photo != null && photo.PhotoId != null)
                    {
                        existingPost.Photos.Add(photo);
                    }

                    return existingPost;
                },
                param: new
                {
                    UserId = userId
                },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }


        public async Task<List<Post>> GetPostWithId(int id, int? userId = null)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetProductsById]",
                (post, photo) =>
                {
                    if (!lookup.TryGetValue(post.Id, out var existingPost))
                    {
                        existingPost = post;
                        existingPost.Photos = new List<Photos>();
                        lookup.Add(existingPost.Id, existingPost);
                    }

                    if (photo != null && photo.PhotoId != null)
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

            return lookup.Values.ToList();
        }


        public async Task<List<Photos>> GetAllPhotos()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<Photos>(
                "[dbo].[GetAllPhotos]",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<bool> likeProduct(int userId, int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var isLiked = await conn.QuerySingleAsync<bool>(
                "[dbo].[LikePost]",
                param: new { UserId = userId, ProductId = productId },
                commandType: CommandType.StoredProcedure
            );

            return isLiked;
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
        public async Task<List<ProductTypes>> GetProductTypes()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<ProductTypes>(
                "[dbo].[GetProductTypes]",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
        public async Task<List<OrderStatuses>> GetOrderStatuses()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<OrderStatuses>(
                "[dbo].[GetOrderStatuses]",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<int> InsertOrderAsync(int userId, int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var orderId = await conn.ExecuteScalarAsync<int>(
                "[dbo].[InsertOrder]",
                new
                {
                    UserId = userId,
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure
            );
            return orderId;
        }

        public async Task<UserOptionalFields> GetUser(int userid)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var result = await conn.QuerySingleAsync<UserOptionalFields>(
                "[dbo].[GetUserOptionalFields]",
                new
                {
                    UserId = userid,
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertPhoneNumber(int userId, string phoneNumber)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@PhoneNumber", phoneNumber);
            parameters.Add("ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await conn.ExecuteAsync(
                "[dbo].[UpdatePhoneNumber]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("ReturnVal");
        }

        public async Task<int> InsertLocation(int userId, string location)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@Location", location);
            parameters.Add("ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await conn.ExecuteAsync(
                "[dbo].[UpdateLocation]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("ReturnVal");
        }
    }
}
