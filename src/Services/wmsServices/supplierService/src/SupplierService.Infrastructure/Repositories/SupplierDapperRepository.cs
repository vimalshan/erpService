using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SupplierService.Application.DTOs;

namespace SupplierService.Infrastructure.Repositories;

public class SupplierDapperRepository
{
    private readonly string _connectionString;

    public SupplierDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<SupplierDto>(
            "SELECT supplier_id AS SupplierId, code AS Code, name AS Name, contact_person AS ContactPerson, " +
            "email AS Email, phone AS Phone, address AS Address, city AS City, state AS State, " +
            "country AS Country, postal_code AS PostalCode, is_active AS IsActive, " +
            "created_date AS CreatedDate, modified_date AS ModifiedDate " +
            "FROM Supplier WHERE supplier_id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<SupplierDto>(
            "SELECT supplier_id AS SupplierId, code AS Code, name AS Name, contact_person AS ContactPerson, " +
            "email AS Email, phone AS Phone, address AS Address, city AS City, state AS State, " +
            "country AS Country, postal_code AS PostalCode, is_active AS IsActive, " +
            "created_date AS CreatedDate, modified_date AS ModifiedDate " +
            "FROM Supplier ORDER BY name");
    }

    public async Task<(IEnumerable<SupplierDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search = null)
    {
        using var connection = new SqlConnection(_connectionString);

        var whereClause = string.IsNullOrWhiteSpace(search) ? "" : "WHERE name LIKE @Search OR code LIKE @Search";
        var searchParam = string.IsNullOrWhiteSpace(search) ? null : $"%{search}%";

        var countSql = $"SELECT COUNT(*) FROM Supplier {whereClause}";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { Search = searchParam });

        var dataSql = $"SELECT supplier_id AS SupplierId, code AS Code, name AS Name, contact_person AS ContactPerson, " +
            "email AS Email, phone AS Phone, address AS Address, city AS City, state AS State, " +
            "country AS Country, postal_code AS PostalCode, is_active AS IsActive, " +
            "created_date AS CreatedDate, modified_date AS ModifiedDate " +
            $"FROM Supplier {whereClause} ORDER BY name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var items = await connection.QueryAsync<SupplierDto>(dataSql, new { Search = searchParam, Offset = (page - 1) * pageSize, PageSize = pageSize });

        return (items, totalCount);
    }
}
