using marusa_line.Dtos;
using marusa_line.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControlPanelController : Controller
    {
        private readonly ProductInterface _postService;
        public ControlPanelController(ProductInterface postService)
        {
            _postService = postService;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetPostsForAdminPanel(int productId, int? userid)
        {

            try
            {
                var posts = await _postService.GetPostsForAdminPanel(productId, userid);

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

        [HttpGet("get-post-byid-controlpanel")]
        public async Task<IActionResult> GetPostByIdControlPanel(int id, int? userid)
        {
            try
            {
                var posts = await _postService.GetPostWithIdControlPanel(id, userid);

                if (posts == null)
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
        [HttpPost("remove-post")]
        public async Task<IActionResult> RemovePost(int postid)
        {
            try
            {
                var posts = await _postService.RemoveProductById(postid);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("revert-post")]
        public async Task<IActionResult> RevertPost(int postid)
        {
            try
            {
                var posts = await _postService.RevertProductById(postid);
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

        [HttpGet("get-like-count")]
        public async Task<IActionResult> GetLikeCount()
        {
            try
            {
                var posts = await _postService.GetLikeCount();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-orders")]
        public async Task<IActionResult> GetOrdersControlPanel(GetOrdersControlPanelDto dto)
        {
            try
            {
                var posts = await _postService.GetOrdersControlPanel(dto);

                if (posts == null || !posts.Any())
                {
                    return Ok(null
                        );
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
