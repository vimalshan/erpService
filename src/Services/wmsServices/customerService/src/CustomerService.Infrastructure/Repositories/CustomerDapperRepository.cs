using CustomerService.Application.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Infrastructure.Repositories;

public class CustomerDapperRepository
{
    private readonly string _connectionString;

    public CustomerDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT customer_id AS CustomerId, code AS Code, name AS Name, company_name AS CompanyName,
                   contact_person AS ContactPerson, contact_title AS ContactTitle, email AS Email,
                   phone AS Phone, address AS Address, city AS City, state AS State,
                   country AS Country, postal_code AS PostalCode, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Customer ORDER BY name
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<CustomerDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return result.ToList().AsReadOnly();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT customer_id AS CustomerId, code AS Code, name AS Name, company_name AS CompanyName,
                   contact_person AS ContactPerson, contact_title AS ContactTitle, email AS Email,
                   phone AS Phone, address AS Address, city AS City, state AS State,
                   country AS Country, postal_code AS PostalCode, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Customer WHERE customer_id = @Id
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<CustomerDto>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<(IReadOnlyList<CustomerDto> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null, CancellationToken cancellationToken = default)
    {
        var whereClause = string.IsNullOrWhiteSpace(search)
            ? ""
            : "WHERE name LIKE @Search OR code LIKE @Search OR company_name LIKE @Search";

        var countSql = $"SELECT COUNT(*) FROM Customer {whereClause}";
        var dataSql = $"""
            SELECT customer_id AS CustomerId, code AS Code, name AS Name, company_name AS CompanyName,
                   contact_person AS ContactPerson, contact_title AS ContactTitle, email AS Email,
                   phone AS Phone, address AS Address, city AS City, state AS State,
                   country AS Country, postal_code AS PostalCode, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Customer {whereClause}
            ORDER BY name
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        var parameters = new
        {
            Search = $"%{search}%",
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        };

        await using var connection = new SqlConnection(_connectionString);
        var totalCount = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var items = await connection.QueryAsync<CustomerDto>(new CommandDefinition(dataSql, parameters, cancellationToken: cancellationToken));

        return (items.ToList().AsReadOnly(), totalCount);
    }
}
