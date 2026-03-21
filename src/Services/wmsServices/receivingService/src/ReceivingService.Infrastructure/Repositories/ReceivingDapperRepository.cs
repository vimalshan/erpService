using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ReceivingService.Application.DTOs;

namespace ReceivingService.Infrastructure.Repositories;

/// <summary>
/// Read-only Dapper repository – used for lightweight query scenarios such as
/// reporting and list endpoints where EF Core overhead is unnecessary.
/// </summary>
public sealed class ReceivingDapperRepository
{
    private readonly string _connectionString;

    public ReceivingDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("ReceivingDb")
            ?? throw new InvalidOperationException("Connection string 'ReceivingDb' not found.");

    public async Task<ReceivingDto?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                r.receiving_id    AS Id,
                r.receiving_number AS ReceivingNumber,
                r.po_id           AS PoId,
                r.warehouse_id    AS WarehouseId,
                r.received_date   AS ReceivedDate,
                r.status          AS Status,
                r.notes           AS Notes,
                r.created_by      AS CreatedBy,
                r.created_date    AS CreatedDate,
                l.receiving_line_id AS Id,
                l.receiving_id    AS ReceivingId,
                l.po_line_id      AS PoLineId,
                l.product_id      AS ProductId,
                l.bin_id          AS BinId,
                l.quantity_received AS QuantityReceived,
                l.lot_number      AS LotNumber,
                l.expiry_date     AS ExpiryDate,
                l.notes           AS Notes
            FROM Receiving r
            LEFT JOIN ReceivingLine l ON l.receiving_id = r.receiving_id
            WHERE r.receiving_id = @id
            """;

        await using var conn = new SqlConnection(_connectionString);
        ReceivingDto? result = null;

        await conn.QueryAsync<dynamic, dynamic, ReceivingDto>(
            sql,
            (header, line) =>
            {
                if (result is null)
                {
                    result = new ReceivingDto(
                        header.Id,
                        header.ReceivingNumber,
                        (int)header.PoId,
                        (int)header.WarehouseId,
                        (DateTime)header.ReceivedDate,
                        header.Status,
                        (string?)header.Notes,
                        (string?)header.CreatedBy,
                        (DateTime)header.CreatedDate,
                        new List<ReceivingLineDto>()
                    );
                }
                if (line?.Id is not null)
                {
                    ((List<ReceivingLineDto>)result.Lines).Add(new ReceivingLineDto(
                        (int)line.Id,
                        (int)line.ReceivingId,
                        (int)line.PoLineId,
                        (int)line.ProductId,
                        (int)line.BinId,
                        (decimal)line.QuantityReceived,
                        (string?)line.LotNumber,
                        line.ExpiryDate is null ? null : DateOnly.FromDateTime((DateTime)line.ExpiryDate),
                        (string?)line.Notes
                    ));
                }
                return result!;
            },
            new { id },
            splitOn: "Id");

        return result;
    }

    public async Task<IEnumerable<ReceivingDto>> GetSummaryListAsync(
        int page = 1, int pageSize = 20)
    {
        const string sql = """
            SELECT
                receiving_id    AS Id,
                receiving_number AS ReceivingNumber,
                po_id           AS PoId,
                warehouse_id    AS WarehouseId,
                received_date   AS ReceivedDate,
                status          AS Status,
                notes           AS Notes,
                created_by      AS CreatedBy,
                created_date    AS CreatedDate
            FROM Receiving
            ORDER BY created_date DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY
            """;

        await using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<dynamic>(sql, new
        {
            offset   = (page - 1) * pageSize,
            pageSize
        });

        return rows.Select(r => new ReceivingDto(
            (int)r.Id,
            r.ReceivingNumber,
            (int)r.PoId,
            (int)r.WarehouseId,
            (DateTime)r.ReceivedDate,
            r.Status,
            (string?)r.Notes,
            (string?)r.CreatedBy,
            (DateTime)r.CreatedDate,
            Array.Empty<ReceivingLineDto>()
        ));
    }
}
