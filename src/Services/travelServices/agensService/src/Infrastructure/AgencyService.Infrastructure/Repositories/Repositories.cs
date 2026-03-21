using AgencyService.Domain.Entities;
using AgencyService.Domain.Repositories;
using AgencyService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgencyService.Infrastructure.Repositories;

public class AgencyRepository : IAgencyRepository
{
    private readonly AgencyDbContext _context;
    
    public AgencyRepository(AgencyDbContext context)
    {
        _context = context;
    }
    
    public async Task<Agency?> GetByCodeAsync(long agencyCode)
    {
        return await _context.Agencies
            .FirstOrDefaultAsync(a => a.AgencyCode == agencyCode);
    }
    
    public async Task<IEnumerable<Agency>> GetAllAsync()
    {
        return await _context.Agencies.ToListAsync();
    }
    
    public async Task<IEnumerable<Agency>> GetByTypeAsync(string type)
    {
        return await _context.Agencies
            .Where(a => a.Type.Code == type)
            .ToListAsync();
    }
    
    public async Task AddAsync(Agency agency)
    {
        await _context.Agencies.AddAsync(agency);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Agency agency)
    {
        _context.Agencies.Update(agency);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(long agencyCode)
    {
        var agency = await GetByCodeAsync(agencyCode);
        if (agency != null)
        {
            _context.Agencies.Remove(agency);
            await _context.SaveChangesAsync();
        }
    }
}

public class VendorRepository : IVendorRepository
{
    private readonly AgencyDbContext _context;
    
    public VendorRepository(AgencyDbContext context)
    {
        _context = context;
    }
    
    public async Task<Vendor?> GetByIdAsync(long vendorId)
    {
        return await _context.Vendors
            .FirstOrDefaultAsync(v => v.Id == vendorId);
    }
    
    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        return await _context.Vendors.ToListAsync();
    }
    
    public async Task<IEnumerable<Vendor>> GetByCategoryAsync(string categoryType)
    {
        return await _context.Vendors
            .Where(v => v.CategoryType == categoryType)
            .ToListAsync();
    }
    
    public async Task AddAsync(Vendor vendor)
    {
        await _context.Vendors.AddAsync(vendor);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Vendor vendor)
    {
        _context.Vendors.Update(vendor);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(long vendorId)
    {
        var vendor = await GetByIdAsync(vendorId);
        if (vendor != null)
        {
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
        }
    }
}

public class AirlineRepository : IAirlineRepository
{
    private readonly AgencyDbContext _context;
    
    public AirlineRepository(AgencyDbContext context)
    {
        _context = context;
    }
    
    public async Task<Airline?> GetByCodeAsync(string code)
    {
        return await _context.Airlines
            .FirstOrDefaultAsync(a => a.Code == code);
    }
    
    public async Task<IEnumerable<Airline>> GetAllAsync()
    {
        return await _context.Airlines.ToListAsync();
    }
    
    public async Task AddAsync(Airline airline)
    {
        await _context.Airlines.AddAsync(airline);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Airline airline)
    {
        _context.Airlines.Update(airline);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string code)
    {
        var airline = await GetByCodeAsync(code);
        if (airline != null)
        {
            _context.Airlines.Remove(airline);
            await _context.SaveChangesAsync();
        }
    }
}
