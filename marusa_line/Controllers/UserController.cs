using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using Google.Apis.Auth;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace marusa_line.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public UserController(IHttpClientFactory _httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = _httpClientFactory;
            _config = config;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var clientId = "834030996453-7av01o52c0tuq6td3ttskd4aqgdu203s.apps.googleusercontent.com";
            var clientSecret = "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";
            var redirectUri = "http://localhost:4200/";

            var state = Guid.NewGuid().ToString("N");
            HttpContext.Session.SetString("oauth_state", state);

            var query = new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["redirect_uri"] = redirectUri!,
                ["response_type"] = "code",
                ["scope"] = "openid profile email",
                ["state"] = state,
                ["access_type"] = "offline",
                ["prompt"] = "consent"
            };

            var url = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", query);
            return Redirect(url);
        }


        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback(string code, string state)
        {
            var clientId = "834030996453-7av01o52c0tuq6td3ttskd4aqgdu203s.apps.googleusercontent.com";
            var clientSecret = "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";
            var expectedState = HttpContext.Session.GetString("oauth_state");
            if (state != expectedState) return BadRequest("Invalid state");

            var client = _httpClientFactory.CreateClient();
            var tokenRequest = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["redirect_uri"] = "http://localhost:4200/",
                ["grant_type"] = "authorization_code"
            };

            var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequest));
            response.EnsureSuccessStatusCode();

            var tokenJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(tokenJson);

            var idToken = doc.RootElement.GetProperty("id_token").GetString();
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var user = new
            {
                Id = payload.Subject,
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture
            };
            var jwt = CreateJwtForUser(user);
            var html = $@"
                <!DOCTYPE html>
                <html>
                <head><title>Google Login</title></head>
                <body>
                <script>
                  window.opener.postMessage({{
                    token: '{jwt}',
                    user: {JsonSerializer.Serialize(user)}
                  }}, '{_config["Frontend:BaseUrl"]}');
                  window.close();
                </script>
                </body>
                </html>";

            return Content(html, "text/html");
        }

        private string CreateJwtForUser(dynamic user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name ?? "")
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
