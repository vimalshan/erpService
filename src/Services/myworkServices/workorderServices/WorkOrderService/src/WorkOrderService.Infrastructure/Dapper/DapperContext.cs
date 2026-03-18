using System.Data;
using Microsoft.Data.SqlClient;
using WorkOrderService.Application.Interfaces;

namespace WorkOrderService.Infrastructure.Dapper;

public class DapperContext : IDapperContext
{
    private readonly string _connectionString;

    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
