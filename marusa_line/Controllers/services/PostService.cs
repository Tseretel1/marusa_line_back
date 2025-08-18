using marusa_line.Controllers.interfaces;
using marusa_line.Controllers.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.IdentityModel.Tokens;

namespace marusa_line.Controllers.services
{
    public class PostService :PostInterface
    {
        private readonly string _connectionString;

        public PostService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("marusa_line_connection");
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
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

                    if (photo != null && photo.PhotoId!=null)
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

        public async Task<bool> likePost(int postId, string emoji)
        {
            throw new NotImplementedException();
        }
    }

}
