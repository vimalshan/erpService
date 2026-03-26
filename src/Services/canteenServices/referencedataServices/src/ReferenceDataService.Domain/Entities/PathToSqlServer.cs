using ReferenceDataService.Domain.Common;

namespace ReferenceDataService.Domain.Entities;

public class PathToSqlServer : BaseEntity
{
    public int Id { get; private set; }
    public string? CompanyCode { get; private set; }
    public string? ServerName { get; private set; }
    public string? DatabaseName { get; private set; }
    public string? UserId { get; private set; }
    public string? DbPassword { get; private set; }

    private PathToSqlServer() { }

    public PathToSqlServer(string? companyCode, string? serverName, string? databaseName, string? userId, string? dbPassword)
    {
        CompanyCode = companyCode;
        ServerName = serverName;
        DatabaseName = databaseName;
        UserId = userId;
        DbPassword = dbPassword;
    }

    public void Update(string? serverName, string? databaseName, string? userId, string? dbPassword)
    {
        ServerName = serverName;
        DatabaseName = databaseName;
        UserId = userId;
        DbPassword = dbPassword;
    }
}
