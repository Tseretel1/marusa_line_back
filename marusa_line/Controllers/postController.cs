using marusa_line.Controllers.interfaces;
using marusa_line.Controllers.services;
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


    }
}
