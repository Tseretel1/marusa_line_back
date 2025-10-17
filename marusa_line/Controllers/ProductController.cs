using marusa_line.interfaces;
using marusa_line.Dtos;
using marusa_line.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductInterface _postService;
        public ProductController(ProductInterface postService)
        {
            _postService = postService;
        }

        [HttpGet("get-posts")]
        public async Task<IActionResult> GetPosts(int productId,int? userid)
        {

            try
            {
                var posts = await _postService.GetPostsAsync(productId,userid); 

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

        [HttpGet("get-posts-for-adminpanel")]
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
        [HttpGet("get-user-liked-posts")]
        public async Task<IActionResult> GetPosts(int userid)
        {

            try
            {
                var posts = await _postService.GetUserLikedPosts(userid);

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
        public async Task<IActionResult> GetMostDiscountedPosts(int? userid)
        {
            try
            {
                
                var posts = await _postService.GetMostDiscountedPosts(userid);

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

        [HttpGet("get-most-sold-posts")]
        public async Task<IActionResult> GetMostSold(int? userid)
        {
            try
            {

                var posts = await _postService.GetMostSoldProducts(userid);

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
        public async Task<IActionResult> GetPostWitId(int id, int? userid)
        {
            try
            {
                var posts = await _postService.GetPostWithId(id,userid);

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
        [HttpGet("get-order-statuses")]
        public async Task<IActionResult> GetOrderStatuses()
        {
            try
            {
                var posts = await _postService.GetOrderStatuses();

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
        public async Task<IActionResult> LikePost(int userid, int productid)
        {
            try
            {
                var posts = await _postService.likeProduct(userid,productid);
                if (posts == false)
                {
                    return Ok(posts);
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

        [HttpPost("insert-order")]
        public async Task<IActionResult> insertOrder(InsertOrder order)
        {
            try
            {
                var posts = await _postService.InsertOrderAsync(order);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-user-orders")]
        public async Task<IActionResult> GetUserOrders(int userid)
        {

            try
            {
                var posts = await _postService.GetUserOrders(userid);

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
        [HttpGet("get-order-details")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {

            try
            {
                var orders= await _postService.GetOrderById(orderId);

                if (orders== null)
                {
                    return NotFound();
                }
                else
                {
                    var prodcut = await _postService.GetPostWithId(orders.ProductId,0);
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

        [HttpGet("get-users-optional")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var posts = await _postService.GetUser(id);

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

        [HttpPost("insert-phone")]
        public async Task<IActionResult> InsertPhoneNumber(int userId, string  phone)
        {
            try
            {
                var posts = await _postService.InsertPhoneNumber(userId, phone);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-location")]
        public async Task<IActionResult> InsertLocation(int userId, string location)
        {
            try
            {
                var posts = await _postService.InsertLocation(userId, location);
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
    }
}
