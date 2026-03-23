using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AttendanceService.Infrastructure.Dapper;

public class DapperContext(IConfiguration configuration)
{
    private readonly string _connectionString =
        configuration.GetConnectionString("AttendanceDb")
        ?? throw new InvalidOperationException("Connection string 'AttendanceDb' not found.");

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
