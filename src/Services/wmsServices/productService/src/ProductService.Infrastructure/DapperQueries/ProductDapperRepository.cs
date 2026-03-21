using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductService.Application.DTOs;

namespace ProductService.Infrastructure.DapperQueries;

public class ProductDapperRepository(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        const string sql = """
            SELECT p.product_id AS ProductId, p.sku AS Sku, p.name AS Name, p.description AS Description,
                   p.category_id AS CategoryId, c.category_name AS CategoryName,
                   p.unit_of_measure AS UnitOfMeasure, p.weight_per_unit AS WeightPerUnit,
                   p.volume_per_unit AS VolumePerUnit, p.price AS Price,
                   p.reorder_point AS ReorderPoint, p.reorder_quantity AS ReorderQuantity,
                   p.is_active AS IsActive, p.created_date AS CreatedDate, p.modified_date AS ModifiedDate
            FROM Product p
            LEFT JOIN Category c ON p.category_id = c.category_id
            WHERE p.is_active = 1
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProductDto>(sql);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId)
    {
        const string sql = """
            SELECT p.product_id AS ProductId, p.sku AS Sku, p.name AS Name, p.description AS Description,
                   p.category_id AS CategoryId, c.category_name AS CategoryName,
                   p.unit_of_measure AS UnitOfMeasure, p.weight_per_unit AS WeightPerUnit,
                   p.volume_per_unit AS VolumePerUnit, p.price AS Price,
                   p.reorder_point AS ReorderPoint, p.reorder_quantity AS ReorderQuantity,
                   p.is_active AS IsActive, p.created_date AS CreatedDate, p.modified_date AS ModifiedDate
            FROM Product p
            LEFT JOIN Category c ON p.category_id = c.category_id
            WHERE p.product_id = @ProductId
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<ProductDto>(sql, new { ProductId = productId });
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        const string sql = """
            SELECT p.product_id AS ProductId, p.sku AS Sku, p.name AS Name, p.description AS Description,
                   p.category_id AS CategoryId, c.category_name AS CategoryName,
                   p.unit_of_measure AS UnitOfMeasure, p.weight_per_unit AS WeightPerUnit,
                   p.volume_per_unit AS VolumePerUnit, p.price AS Price,
                   p.reorder_point AS ReorderPoint, p.reorder_quantity AS ReorderQuantity,
                   p.is_active AS IsActive, p.created_date AS CreatedDate, p.modified_date AS ModifiedDate
            FROM Product p
            LEFT JOIN Category c ON p.category_id = c.category_id
            WHERE p.category_id = @CategoryId AND p.is_active = 1
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProductDto>(sql, new { CategoryId = categoryId });
    }
}
