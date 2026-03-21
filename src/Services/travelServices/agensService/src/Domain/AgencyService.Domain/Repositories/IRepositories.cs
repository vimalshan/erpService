namespace AgencyService.Domain.Repositories;

public interface IAgencyRepository
{
    Task<Entities.Agency?> GetByCodeAsync(long agencyCode);
    Task<IEnumerable<Entities.Agency>> GetAllAsync();
    Task<IEnumerable<Entities.Agency>> GetByTypeAsync(string type);
    Task AddAsync(Entities.Agency agency);
    Task UpdateAsync(Entities.Agency agency);
    Task DeleteAsync(long agencyCode);
}

public interface IVendorRepository
{
    Task<Entities.Vendor?> GetByIdAsync(long vendorId);
    Task<IEnumerable<Entities.Vendor>> GetAllAsync();
    Task<IEnumerable<Entities.Vendor>> GetByCategoryAsync(string categoryType);
    Task AddAsync(Entities.Vendor vendor);
    Task UpdateAsync(Entities.Vendor vendor);
    Task DeleteAsync(long vendorId);
}

public interface IAirlineRepository
{
    Task<Entities.Airline?> GetByCodeAsync(string code);
    Task<IEnumerable<Entities.Airline>> GetAllAsync();
    Task AddAsync(Entities.Airline airline);
    Task UpdateAsync(Entities.Airline airline);
    Task DeleteAsync(string code);
}
