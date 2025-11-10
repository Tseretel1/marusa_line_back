using Dapper;
using System.Data;
using marusa_line.interfaces;
using Microsoft.Data.SqlClient;

namespace marusa_line.services
{
    public class ControlPanelService : ControlPanelInterface
    {
       
        private readonly string _connectionString;

        public ControlPanelService(IConfiguration config)
        {
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
    }
}
