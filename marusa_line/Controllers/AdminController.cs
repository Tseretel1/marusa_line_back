using marusa_line.Dtos.AdminPanelDtos;
using marusa_line.Dtos.ControlPanelDtos.ShopDtos;
using marusa_line.interfaces;
using marusa_line.services;
using Microsoft.AspNetCore.Mvc;

namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly AdminInterface _adminService;
        public AdminController(AdminInterface adminService, ControlPanelInterface controlPanelService)
        {
            _adminService = adminService;
        }
  
        [HttpGet("get-every-shop")]
        public async Task<IActionResult> GetShopbyId()
        {
            try
            {
                var user = await _adminService.GetEveryShop();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-shop-by-id")]
        public async Task<IActionResult> GetShopbyId(int shopId)
        {
            try
            {
                var user = await _adminService.GetShopById(shopId);
                return Ok(user);
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
                var user = await _adminService.GetShopStats(shopId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-shop")]
        public async Task<IActionResult> UpdateShop([FromBody] ShopDto shop)
        {
            try
            {
                var user = await _adminService.UpdateShopAsync(shop);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-subscription")]
        public async Task<IActionResult> updateSubscription(SubscriptionDto subscripion)
        {
            try
            {
                var user = await _adminService.UpdateSubscription(subscripion);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-shop")]
        public async Task<IActionResult> AddShop(AddShop shop)
        {
            try
            {
                var user = await _adminService.AddShop(shop);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdateShopPassword(UpdateShopPassword shop)
        {
            try
            {
                var user = await _adminService.UpdateShopPassword(shop);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
