using Microsoft.Data.SqlClient;
using System.Data;

namespace MasterDataService.Infrastructure.Persistence.Dapper;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}

public class DapperContext(string connectionString) : IDapperContext
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
