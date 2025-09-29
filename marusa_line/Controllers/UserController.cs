using Microsoft.AspNetCore.Mvc;
using marusa_line.Models;
using System.Text;

using marusa_line.interfaces;
namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserInterface _userService;

        public UserController(UserInterface userInterface)
        {
            _userService = userInterface;
        }

        [HttpGet("google")]
        public IActionResult Google()
        {
            var url = _userService.GetGoogleAuthUrl();
            return Redirect(url);
        }



        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            var expectedState = HttpContext.Session.GetString("oauth_state");
            var authResult = await _userService.GoogleCallbackAsync(code, state, expectedState);

            var html = $@"
                 <html>
                     <body>
                         <script>
                             window.opener.postMessage({{
                                 token: '{authResult.Token}',
                                 user: {System.Text.Json.JsonSerializer.Serialize(authResult.User)}
                             }}, 'http://localhost:4200');
                             window.close();
                         </script>
                     </body>
                 </html>
             ";

            return Content(html, "text/html");
        }
    }
}
