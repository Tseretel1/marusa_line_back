using Google.Apis.Auth;
using marusa_line.Dtos;
using marusa_line.interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Dapper;
using marusa_line.Models;


namespace marusa_line.services
{
    public class UserService : UserInterface
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly GoogleSettings _googleSettings;
        private readonly string _connectionString;

        public UserService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _googleSettings = _config.GetSection("web").Get<GoogleSettings>()
                          ?? throw new InvalidOperationException("Missing 'web' config section");
            _connectionString = config.GetConnectionString("marusa_line_connection");

        }
        public string GetGoogleAuthUrl()
        {
            var clientId = _googleSettings.Client_Id!;
            var redirectUri = _googleSettings.Redirect_Uris?.FirstOrDefault()
                              ?? throw new Exception("No redirect uri configured");

            var state = Guid.NewGuid().ToString("N");
            var query = new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["redirect_uri"] = redirectUri,
                ["response_type"] = "code",
                ["scope"] = "openid profile email",
                ["state"] = state,
                ["access_type"] = "offline",
                ["prompt"] = "consent"
            };

            var url = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", query);
            return url;
        }


        public async Task<AuthResultDto?> GoogleCallbackAsync(string code, string state,string expectedState)
        {

            var clientId = _googleSettings.Client_Id!;
            var clientSecret = _googleSettings.Client_Secret!;
            var redirectUri = _googleSettings.Redirect_Uris?.FirstOrDefault()
                              ?? throw new Exception("No redirect uri configured");

            var client = _httpClientFactory.CreateClient();
            var tokenRequest = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["redirect_uri"] = redirectUri,
                ["grant_type"] = "authorization_code"
            };

            var response = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequest));
            response.EnsureSuccessStatusCode();

            var tokenJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(tokenJson);

            var idToken = doc.RootElement.GetProperty("id_token").GetString();
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            var user = new UserDto
            {
                Id = payload.Subject,
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture
            };

            var jwt = CreateJwtForUser(user);

            return new AuthResultDto
            {
                Token = jwt,
                User = user
            };
        }

        private string CreateJwtForUser(UserDto user)
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


        public async Task<bool> InsertUserIfNotExistsAsync(User user)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var exists = await conn.QuerySingleAsync<bool>(
                "[dbo].[UserExistsCheck]",
                param: new { Email = user.Email },
                commandType: CommandType.StoredProcedure
            );

            if (exists)
            {
                return false; 
            }
            var newUserId = await conn.QuerySingleAsync<int>(
                "[dbo].[InsertUser]",
                param: new
                {
                    Name = user.Name,
                    Email = user.Email,
                    ProfilePhoto = user.ProfilePhoto
                },
                commandType: CommandType.StoredProcedure
            );

            user.Id = newUserId;

            return true;
        }
    }
}
