using marusa_line.Dtos;
using marusa_line.Dtos.ControlPanelDtos;
using marusa_line.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControlPanelController : Controller
    {
        private readonly ProductInterface _postService;
        private readonly ControlPanelInterface _controlPanelService;
        public ControlPanelController(ProductInterface postService, ControlPanelInterface controlPanelService)
        {
            _postService = postService;
            _controlPanelService = controlPanelService;
        }

        [HttpPost("get-products")]
        public async Task<IActionResult> GetPostsForAdminPanel(GetPostsDto getPosts)
        {

            try
            {
                var posts = await _controlPanelService.GetPostsForAdminPanel(getPosts);

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
                var orders = await _postService.GetOrdersControlPanel(dto);
                var totalCount = await _controlPanelService.GetOrdersTotalCountAsync(dto.IsPaid);

                if (orders == null || !orders.Any())
                {
                    return Ok(null);
                }
                var returnObj = new
                {
                    orders = orders,
                    totalCount = totalCount,
                };
                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-order-details")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {

            try
            {
                var orders = await _postService.GetOrderById(orderId);

                if (orders == null)
                {
                    return Ok(null);
                }
                else
                {
                    var prodcut = await _postService.GetOrderProduct(orders.ProductId, 0);
                    var returnOrder = new
                    {
                        orders,
                        product = prodcut,
                    };


                    return Ok(returnOrder);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("change-order-ispaid")]
        public async Task<IActionResult> ChangeIsPaid(int orderId,bool ispaid)
        {
            try
            {
                var posts = await _controlPanelService.ToggleOrderIsPaidAsync(orderId,ispaid);
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
        [HttpPost("change-order-status")]
        public async Task<IActionResult> orderStatus(int orderId, int statusId)
        {
            try
            {
                var posts = await _controlPanelService.ChangeOrderStatus(orderId, statusId);
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
        [HttpDelete("delete-order")]
        public async Task<IActionResult> deleteOrder(int orderId)
        {
            try
            {
                var posts = await _controlPanelService.DeleteOrder(orderId);
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(ControlPanelLoginDto dto)
        {

            try
            {
                var token = await _controlPanelService.Login(dto);

                if (token == null)
                {
                    return Ok(null);
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
