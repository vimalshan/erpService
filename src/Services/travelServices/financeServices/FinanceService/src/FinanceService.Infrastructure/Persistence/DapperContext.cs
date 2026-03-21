using System.Data;
using FinanceService.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;

namespace FinanceService.Infrastructure.Persistence;

public class DapperContext : IDapperContext
{
    private readonly string _connectionString;

    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
