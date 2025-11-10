using Dapper;
using System.Data;
using marusa_line.interfaces;
using Microsoft.Data.SqlClient;
using marusa_line.Dtos.ControlPanelDtos;
using marusa_line.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace marusa_line.services
{
    public class ControlPanelService : ControlPanelInterface
    {
       
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public ControlPanelService(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("marusa_line_connection");
        }

        public async Task<int> ToggleOrderIsPaidAsync(int orderId, bool isPaid)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            parameters.Add("@Paid", isPaid, DbType.Boolean);

            var rowsAffected = await conn.ExecuteAsync(
                "[dbo].[ChangeOrderIsPaid]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
        public async Task<int> ChangeOrderStatus(int orderId, int isPaid)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            parameters.Add("@StatusId", isPaid, DbType.Int32);

            var rowsAffected = await conn.ExecuteAsync(
                "[dbo].[ChangeOrderStatus]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }
        public async Task<int> GetOrdersTotalCountAsync(bool? isPaid)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@Paid", isPaid, DbType.Boolean);

            var totalCount = await conn.QuerySingleAsync<int>(
                "[dbo].[getOrdersTotalCount]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return totalCount;
        }

        public async Task<int> DeleteOrder(int orderId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            var rowsAffected = await conn.ExecuteAsync(
                "[dbo].[DeleteOrder]",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected;
        }

        public async Task<ControlPanelLoginReturn> Login(ControlPanelLoginDto loginDto)
        {
            if(loginDto.Username =="giorgi"&& loginDto.Password == "giorgi")
            {
                var jwtToken = CreateJwtForUser(loginDto.Username);
                var ControlPanelReturn = new ControlPanelLoginReturn
                {
                    Token = jwtToken,
                };
                return ControlPanelReturn;
            }
            return null;
        }


        private string CreateJwtForUser(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("username", username),
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
