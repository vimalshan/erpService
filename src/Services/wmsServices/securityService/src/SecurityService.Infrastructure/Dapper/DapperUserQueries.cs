using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecurityService.Application.DTOs;

namespace SecurityService.Infrastructure.Dapper;

public interface IDapperUserQueries
{
    Task<UserDto?> GetUserWithRolesAsync(int userId);
    Task<IReadOnlyList<UserDto>> GetActiveUsersAsync();
}

public class DapperUserQueries : IDapperUserQueries
{
    private readonly string _connectionString;

    public DapperUserQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SecurityDb")!;
    }

    public async Task<UserDto?> GetUserWithRolesAsync(int userId)
    {
        const string sql = """
            SELECT u.UserID, u.Username, u.Email, u.FullName, u.IsActive, u.CreatedDate, u.LastLogin
            FROM Users u WHERE u.UserID = @UserId;
            
            SELECT r.RoleName FROM Roles r
            INNER JOIN UserRoles ur ON ur.RoleID = r.RoleID
            WHERE ur.UserID = @UserId;
            """;

        await using var connection = new SqlConnection(_connectionString);
        using var multi = await connection.QueryMultipleAsync(sql, new { UserId = userId });

        var user = await multi.ReadFirstOrDefaultAsync<dynamic>();
        if (user is null) return null;

        var roles = (await multi.ReadAsync<string>()).ToList();

        return new UserDto(user.UserID, user.Username, user.Email, user.FullName, user.IsActive, user.CreatedDate, user.LastLogin, roles);
    }

    public async Task<IReadOnlyList<UserDto>> GetActiveUsersAsync()
    {
        const string sql = """
            SELECT u.UserID, u.Username, u.Email, u.FullName, u.IsActive, u.CreatedDate, u.LastLogin
            FROM Users u WHERE u.IsActive = 1;
            """;

        await using var connection = new SqlConnection(_connectionString);
        var users = await connection.QueryAsync<dynamic>(sql);

        return users.Select(u => new UserDto(
            (int)u.UserID, (string)u.Username, (string)u.Email, (string)u.FullName,
            (bool)u.IsActive, (DateTime)u.CreatedDate, (DateTime?)u.LastLogin, new List<string>()
        )).ToList();
    }
}
