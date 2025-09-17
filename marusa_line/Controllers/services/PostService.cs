using marusa_line.Controllers.interfaces;
using marusa_line.Controllers.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using marusa_line.Controllers.NewFolder;

namespace marusa_line.Controllers.services
{
    public class PostService : PostInterface
    {
        private readonly string _connectionStrings;

        public PostService(IConfiguration config)
        {
            _connectionStrings = config.GetConnectionString("marusa_line_connection");
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            using var conn = new SqlConnection(_connectionStrings);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetPostsWithPhotos]",
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
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }


        public async Task<List<Post>> GetMostDiscountedPosts()
        {
            using var conn = new SqlConnection(_connectionStrings);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetTopDiscountedPostsWithPhotos]",
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
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }
        public async Task<List<Post>> GetPostWithId(int id)
        {
            using var conn = new SqlConnection(_connectionStrings);
            await conn.OpenAsync();

            var lookup = new Dictionary<int, Post>();

            var result = await conn.QueryAsync<Post, Photos, Post>(
                "[dbo].[GetPostsWithPhotosWithId]",
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
                param: new { Id = id },
                splitOn: "PhotoId",
                commandType: CommandType.StoredProcedure
            );

            return lookup.Values.ToList();
        }

        public async Task<List<Photos>> GetAllPhotos()
        {
            using var conn = new SqlConnection(_connectionStrings);
            await conn.OpenAsync();

            var result = await conn.QueryAsync<Photos>(
                "[dbo].[GetAllPhotos]",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<bool> likePost(int postId, string emoji)
        {
            throw new NotImplementedException();
        }
        public async Task<int> InsertPostAsync(InsertPostDto dto)
        {
            using var conn = new SqlConnection(_connectionStrings);
            await conn.OpenAsync();
            var postId = await conn.ExecuteScalarAsync<int>(
                "[dbo].[InsertPostWithPhotos]",
                new
                {
                    dto.Title,
                    dto.Description,
                    dto.Price,
                    dto.DiscountedPrice,
                    dto.Quantity,
                    dto.ProductType
                },
                commandType: CommandType.StoredProcedure
            );
            foreach (var photo in dto.Photos)
            {
                await conn.ExecuteAsync(
                    "[dbo].[InsertPhoto]",
                    new { PostId = postId, photo.PhotoUrl },
                    commandType: CommandType.StoredProcedure
                );
            }
            return postId;
        }
        public async Task<int> EditPostAsync(InsertPostDto dto)
        {
            using var conn = new SqlConnection(_connectionStrings);
            await conn.OpenAsync();
            var postId = await conn.ExecuteScalarAsync<int>(
                "[dbo].[EditPostWithPhotos]",
                new
                {
                    dto.Id,
                    dto.Title,
                    dto.Description,
                    dto.Price,
                    dto.DiscountedPrice,
                    dto.Quantity,
                    dto.ProductType
                },
                commandType: CommandType.StoredProcedure
            );
            foreach (var photo in dto.Photos)
            {
                await conn.ExecuteAsync(
                    "[dbo].[InsertPhoto]",
                    new { PostId = postId, photo.PhotoUrl },
                    commandType: CommandType.StoredProcedure
                );
            }
            return postId;
        }
        public async Task<DateTime> deletePhoto(int photoId)
        {
            using var conn = new SqlConnection(_connectionStrings);
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
    }

}