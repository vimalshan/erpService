using Dapper;
using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.Infrastructure.Dapper;

public class WarehouseDapperRepository
{
    private readonly DapperContext _context;

    public WarehouseDapperRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WarehouseDto>> GetAllWarehousesAsync()
    {
        const string sql = @"
            SELECT warehouse_id AS WarehouseId, code AS Code, name AS Name,
                   address AS Address, city AS City, state AS State,
                   country AS Country, postal_code AS PostalCode,
                   phone AS Phone, email AS Email, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Warehouse";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<WarehouseDto>(sql);
    }

    public async Task<WarehouseDto?> GetWarehouseByIdAsync(int id)
    {
        const string sql = @"
            SELECT warehouse_id AS WarehouseId, code AS Code, name AS Name,
                   address AS Address, city AS City, state AS State,
                   country AS Country, postal_code AS PostalCode,
                   phone AS Phone, email AS Email, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Warehouse WHERE warehouse_id = @Id";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<WarehouseDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<ZoneDto>> GetZonesByWarehouseIdAsync(int warehouseId)
    {
        const string sql = @"
            SELECT zone_id AS ZoneId, warehouse_id AS WarehouseId,
                   code AS Code, name AS Name, zone_type AS ZoneType,
                   description AS Description, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Zone WHERE warehouse_id = @WarehouseId";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<ZoneDto>(sql, new { WarehouseId = warehouseId });
    }
}
