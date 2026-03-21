using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Interfaces;
using RackingSystem.Infrastructure.Persistence;

namespace RackingSystem.Infrastructure.Repositories;

public sealed class BinRepository : IBinRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public BinRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");
    }

    public async Task<Bin?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Bins.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<Bin?> GetByBarcodeAsync(string barcode, CancellationToken ct = default) =>
        await _context.Bins.FirstOrDefaultAsync(b => b.Barcode == barcode, ct);

    public async Task<IEnumerable<Bin>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Bins.AsNoTracking().Where(b => b.IsActive).ToListAsync(ct);

    public async Task<IEnumerable<Bin>> GetByZoneIdAsync(int zoneId, CancellationToken ct = default) =>
        await _context.Bins.AsNoTracking().Where(b => b.ZoneId == zoneId && b.IsActive).ToListAsync(ct);

    public async Task<IEnumerable<Bin>> GetByShelfIdAsync(int shelfId, CancellationToken ct = default) =>
        await _context.Bins.AsNoTracking().Where(b => b.ShelfId == shelfId && b.IsActive).ToListAsync(ct);

    public async Task<IEnumerable<Bin>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        var upperStatus = status.ToUpperInvariant();
        return await _context.Bins.AsNoTracking().Where(b => b.Status == upperStatus && b.IsActive).ToListAsync(ct);
    }

    /// <summary>Uses Dapper to call the SQL function fn_GetBinUtilization.</summary>
    public async Task<decimal?> GetBinUtilizationAsync(int binId, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.ExecuteScalarAsync<decimal?>(
            "SELECT dbo.fn_GetBinUtilization(@bin_id)",
            new { bin_id = binId });
        return result;
    }

    public async Task AddAsync(Bin bin, CancellationToken ct = default) =>
        await _context.Bins.AddAsync(bin, ct);

    public void Update(Bin bin) => _context.Bins.Update(bin);
    public void Remove(Bin bin) => _context.Bins.Remove(bin);
}
