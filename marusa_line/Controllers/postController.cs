using marusa_line.interfaces;
using marusa_line.Dtos;
using marusa_line.services;
using Microsoft.AspNetCore.Mvc;

namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class postController : Controller
    {
        private readonly PostInterface _postService;

        public postController(PostInterface postService)
        {
            _postService = postService;
        }

        [HttpGet("get-posts")]
        public async Task<IActionResult> GetPosts()
        {

            try
            {
                var posts = await _postService.GetPostsAsync(); 

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-most-discounted-posts")]
        public async Task<IActionResult> GetMostDiscountedPosts()
        {
            try
            {
                var posts = await _postService.GetMostDiscountedPosts();

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-post-with-id")]
        public async Task<IActionResult> GetPostWitId(int id)
        {
            try
            {
                var posts = await _postService.GetPostWithId(id);

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-all-photos")]
        public async Task<IActionResult> GetAllPhotos()
        {
            try
            {
                var posts = await _postService.GetAllPhotos();

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product-types")]
        public async Task<IActionResult> GetProductTypes()
        {
            try
            {
                var posts = await _postService.GetProductTypes();

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("like-post")]
        public async Task<IActionResult> LikePost()
        {
            try
            {
                var posts = await _postService.GetAllPhotos();

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-post")]
        public async Task<IActionResult> InsertPostAsync([FromBody] InsertPostDto dto)
        {
            try
            {
                var posts = await _postService.InsertPostAsync(dto);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("edit-post")]
        public async Task<IActionResult> EditPostAsync([FromBody] InsertPostDto dto)
        {
            try
            {
                var posts = await _postService.EditPostAsync(dto);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("delete-photo")]
        public async Task<IActionResult> deletePhoto(int photoId)
        {
            try
            {
                var posts = await _postService.deletePhoto(photoId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
