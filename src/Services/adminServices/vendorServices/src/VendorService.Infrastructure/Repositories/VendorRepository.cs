using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VendorService.Domain.Entities;
using VendorService.Domain.Interfaces;
using VendorService.Infrastructure.Data;

namespace VendorService.Infrastructure.Repositories;

public sealed class VendorRepository : IVendorRepository
{
    private readonly VendorDbContext _context;
    private readonly string _connectionString;

    public VendorRepository(VendorDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("VendorDb")
            ?? throw new InvalidOperationException("Connection string 'VendorDb' not configured.");
    }

    public async Task<VendorMaster?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await _context.VendorMasters.FirstOrDefaultAsync(v => v.Id == id, ct);

    public async Task<IEnumerable<VendorMaster>> GetAllAsync(CancellationToken ct = default) =>
        await _context.VendorMasters.ToListAsync(ct);

    public async Task<IEnumerable<VendorMaster>> GetByStatusAsync(char status, CancellationToken ct = default) =>
        await _context.VendorMasters
            .Where(v => EF.Property<string>(v, "VM_LIVESTATUS") == status.ToString())
            .ToListAsync(ct);

    public async Task<IEnumerable<VendorMaster>> GetByLocationAsync(long locationId, CancellationToken ct = default) =>
        await _context.VendorMasters
            .Where(v => v.LocationId == locationId)
            .ToListAsync(ct);

    public async Task AddAsync(VendorMaster vendor, CancellationToken ct = default) =>
        await _context.VendorMasters.AddAsync(vendor, ct);

    public void Update(VendorMaster vendor) =>
        _context.VendorMasters.Update(vendor);

    public void Remove(VendorMaster vendor) =>
        _context.VendorMasters.Remove(vendor);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);

    public async Task<long> AddUpdateVendorSpAsync(
        long? vendorId, long categoryId, long locationId, string name, string? email,
        string address, long updatedBy, char liveStatus, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@p_VM_ID", vendorId, System.Data.DbType.Int64);
        parameters.Add("@p_VM_CATID", categoryId);
        parameters.Add("@p_VM_LOC_ID", locationId);
        parameters.Add("@p_VM_NAME", name);
        parameters.Add("@p_VM_EMAIL", email);
        parameters.Add("@p_VM_ADDRESS", address);
        parameters.Add("@p_UpdatedBy", updatedBy);
        parameters.Add("@p_VM_LIVESTATUS", liveStatus.ToString());

        await connection.ExecuteAsync(
            "dbo.usp_AddUpdateVendor",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure);

        // Return the vendor ID (if inserting, fetch the newly created one)
        if (vendorId.HasValue) return vendorId.Value;

        var newId = await connection.ExecuteScalarAsync<long>(
            "SELECT MAX(VM_ID) FROM VENDOR_MASTER");
        return newId;
    }
}
