using marusa_line.Dtos;
using marusa_line.Dtos.ControlPanelDtos;
using marusa_line.Dtos.ControlPanelDtos.Dashboard;
using marusa_line.Dtos.ControlPanelDtos.User;
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
                var posts = await _controlPanelService.GetPostWithIdControlPanel(id, userid);

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
                var posts = await _controlPanelService.InsertPostAsync(dto);
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
                var posts = await _controlPanelService.EditPostAsync(dto);
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
                var posts = await _controlPanelService.RemoveProductById(postid);
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
                var posts = await _controlPanelService.RevertProductById(postid);
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
                var posts = await _controlPanelService.deletePhoto(photoId);
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
                var posts = await _controlPanelService.GetLikeCount();
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
                var orders = await _controlPanelService.GetOrdersControlPanel(dto);
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
        [HttpPost("get-statistics")]
        public async Task<IActionResult> GetStatistics(GetDahsboard dto)
        {
            try
            {
                var statistics = await _controlPanelService.GetDashboardStatistics(dto);
                if (statistics == null)
                {
                    return Ok(null);
                }
                var returnObj = new
                {
                    statistics = statistics,
                };
                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-dashboard-by-year")]
        public async Task<IActionResult> GetDashboard(int year)
        {
            try
            {
                var statistics = await _controlPanelService.GetDashboard(year);
                if (statistics == null)
                {
                    return Ok(null);
                }

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-sold-producttypes")]
        public async Task<IActionResult> GetSoldProductTypes(int year, int? month)
        {
            try
            {
                var statistics = await _controlPanelService.GetSoldProductTypes(year,month);
                if (statistics == null)
                {
                    return Ok(null);
                }

                return Ok(statistics);
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
        public async Task<IActionResult> ChangeIsPaid(int orderId,bool ispaid, int quantity)
        {
            try
            {
                var posts = await _controlPanelService.ToggleOrderIsPaidAsync(orderId,ispaid, quantity);
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

        [HttpPost("insert-product-type")]
        public async Task<IActionResult> InsertProductType(string productType)
        {
            try
            {
                var productTypes = await _controlPanelService.InsertProducType(productType);
                if (productTypes == null)
                {
                    return Ok(null);
                }
                var returnObj = new
                {
                    productTypes = productTypes,
                };
                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("edit-product-type")]
        public async Task<IActionResult> EditProductType(int id, string productType)
        {
            try
            {
                var productTypes = await _controlPanelService.EditProductType(id,productType);
                if (productTypes == null)
                {
                    return Ok(null);
                }
                var returnObj = new
                {
                    productTypes = productTypes,
                };
                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-product-type")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            try
            {
                var productTypes = await _controlPanelService.DeleteProductType(id);
                if (productTypes == null)
                {
                    return Ok(null);
                }
                var returnObj = new
                {
                    productTypes = productTypes,
                };
                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("get-users")]
        public async Task<IActionResult> GetUsers(GetUserFilteredDto dto)
        {
            try
            {
                var users = await _controlPanelService.GetUsersList(dto);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _controlPanelService.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-user-by-name")]
        public async Task<IActionResult> GetsuerByName(string search)
        {
            try
            {
                var statistics = await _controlPanelService.SearchUserByName(search);
                if (statistics == null)
                {
                    return Ok(null);
                }

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-user-by-email")]
        public async Task<IActionResult> GetsuerByEmail(string search)
        {
            try
            {
                var statistics = await _controlPanelService.SearchUserByEmail(search);
                if (statistics == null)
                {
                    return Ok(null);
                }

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-user-role")]
        public async Task<IActionResult> UpdateUserRole(int id,string role)
        {
            try
            {
                var user = await _controlPanelService.UpdateUserRole(id, role);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-product-order-allowed")]
        public async Task<IActionResult> UpdateProductOderAllowed(int productID, bool allowed)
        {
            try
            {
                var user = await _controlPanelService.UpdateProductOderAllowed(productID, allowed);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        


    }
}
