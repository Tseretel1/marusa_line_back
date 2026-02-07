using marusa_line.interfaces;
using marusa_line.Dtos;
using marusa_line.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

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


        [HttpPost("get-posts")]
        public async Task<IActionResult> GetPosts(GetProductDto dto)
        {

            try
            {
                var posts = await _postService.GetPostsAsync(dto); 

                if (posts == null)
                {
                    return Ok(null);
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("get-user-liked-posts")]
        public async Task<IActionResult> GetPosts(int userid)
        {

            try
            {
                var posts = await _postService.GetUserLikedPosts(userid);

                if (posts == null || !posts.Any())
                {
                    return Ok(null);
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
                    return Ok(null);
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
                    return Ok(null);
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
                    return Ok(null);
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
                    return Ok(null);
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product-types")]
        public async Task<IActionResult> GetProductTypes(int shopid)
        {
            try
            {
                var posts = await _postService.GetProductTypes(shopid);

                if (posts == null || !posts.Any())
                {
                    return Ok(null);
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
                    return Ok(null);
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpGet("get-user-orders")]
        public async Task<IActionResult> GetUserOrders(int userid)
        {

            try
            {
                var posts = await _postService.GetUserOrders(userid);

                if (posts == null || !posts.Any())
                {
                    return Ok(null);
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("get-order-details")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                var orders= await _postService.GetOrderById(orderId);
                if (orders== null)
                {
                    return Ok(null);
                }
                else
                {
                    var prodcut = await _postService.GetOrderProduct(orders.ProductId,0);
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
                    return Ok(null);
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
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

        [Authorize]
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
        [Authorize]
        [HttpPost("follow-shop")]
        public async Task<IActionResult> FollowShop(int userId,int shopId)
        {
            try
            {
                await _postService.FollowShop(userId, shopId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shop-stats")]
        public async Task<IActionResult> UpdateProductOderAllowed(int shopId)
        {
            try
            {
                var user = await _postService.GetShopStats(shopId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-shop-by-id")]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetShopbyId(int shopId)
        {
            try
            {
                var shopIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(shopIdClaim))
                {
                    var user = await _postService.GetShopById(shopId,null);
                    return Ok(user);
                }
                else
                {
                    int userid = int.Parse(shopIdClaim);
                    var user = await _postService.GetShopById(shopId,userid);
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}