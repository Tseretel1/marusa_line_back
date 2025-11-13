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
        private readonly FacebookSettings _facebookSettings;
        private readonly string _connectionString;
        public UserService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;


            _googleSettings = _config.GetSection("web").Get<GoogleSettings>()
                          ?? throw new InvalidOperationException("Missing 'web' config section");
            _facebookSettings = _config.GetSection("Facebook").Get<FacebookSettings>()
                        ?? throw new InvalidOperationException("Missing 'Facebook' config section");


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
            var userInssertion = new UserDto
            {
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture
            };
            var userInsertion = await InsertUserIfNotExistsAsync(userInssertion);


            var user = new UserDto
            {
                Id = userInsertion,
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

        public async Task<int> InsertUserIfNotExistsAsync(UserDto user)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var exists = await conn.QuerySingleAsync<int>(
                "[dbo].[UserExistsCheck]",
                param: new { Email = user.Email },
                commandType: CommandType.StoredProcedure
            );

            if (exists!=0)
            {
                var UpdateUser = await conn.QuerySingleAsync<int>(
                "[dbo].[UpdateUser]",
                  param: new
                  {
                      Name = user.Name,
                      Email = user.Email,
                      ProfilePhoto = user.Picture
                  },
                  commandType: CommandType.StoredProcedure
                );
                return UpdateUser; 
            }
            var newUserId = await conn.QuerySingleAsync<int>(
                "[dbo].[InsertUser]",
                param: new
                {
                    Name = user.Name,
                    Email = user.Email,
                    ProfilePhoto = user.Picture
                },
                commandType: CommandType.StoredProcedure
            );

            user.Id = newUserId;
            return newUserId;
        }

        private string CreateJwtForUser(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
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



        public string GetFacebookAuthUrl()
        {
            var query = new Dictionary<string, string>
            {
                ["client_id"] = _facebookSettings.ClientId,
                ["redirect_uri"] = _facebookSettings.RedirectUri,
                ["response_type"] = "code",
                ["scope"] = "email,public_profile"
            };

            return QueryHelpers.AddQueryString("https://www.facebook.com/v20.0/dialog/oauth", query);
        }
        public async Task<AuthResultDto?> FacebookCallbackAsync(string code)
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Exchange code for access token
            var tokenUrl = QueryHelpers.AddQueryString(
                "https://graph.facebook.com/v20.0/oauth/access_token",
                new Dictionary<string, string>
                {
                    ["client_id"] = _facebookSettings.ClientId,
                    ["redirect_uri"] = _facebookSettings.RedirectUri,
                    ["client_secret"] = _facebookSettings.ClientSecret,
                    ["code"] = code
                });

            var tokenResponse = await client.GetAsync(tokenUrl);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            using var tokenDoc = JsonDocument.Parse(tokenJson);

            var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();

            if (string.IsNullOrEmpty(accessToken))
                throw new Exception("Failed to obtain Facebook access token");

            var userInfoUrl = QueryHelpers.AddQueryString(
                "https://graph.facebook.com/me",
                new Dictionary<string, string>
                {
                    ["fields"] = "id,name,email,picture",
                    ["access_token"] = accessToken
                });

            var userResponse = await client.GetAsync(userInfoUrl);
            userResponse.EnsureSuccessStatusCode();
            var userJson = await userResponse.Content.ReadAsStringAsync();
            using var userDoc = JsonDocument.Parse(userJson);

            var email = userDoc.RootElement.TryGetProperty("email", out var emailProp)
                        ? emailProp.GetString()
                        : $"{userDoc.RootElement.GetProperty("id").GetString()}@facebook.com"; 
            var name = userDoc.RootElement.GetProperty("name").GetString();
            var picture = userDoc.RootElement.GetProperty("picture")
                                             .GetProperty("data")
                                             .GetProperty("url")
                                             .GetString();

            var userInsertion = new UserDto
            {
                Email = email,
                Name = name,
                Picture = picture
            };
            var userId = await InsertUserIfNotExistsAsync(userInsertion);

            var user = new UserDto
            {
                Id = userId,
                Email = email,
                Name = name,
                Picture = picture
            };
            var jwt = CreateJwtForUser(user);
            return new AuthResultDto
            {
                Token = jwt,
                User = user
            };
        }

    }
}
