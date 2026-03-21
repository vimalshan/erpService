using InsuranceService.Domain.Entities;

namespace InsuranceService.Domain.Repositories;

public interface ITravelInsuranceRepository
{
    Task<TravelInsurance?> GetByKeyAsync(string companyCode, long planNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TravelInsurance>> GetAllAsync(string? companyCode = null, CancellationToken cancellationToken = default);
    Task AddAsync(TravelInsurance insurance, CancellationToken cancellationToken = default);
    Task UpdateAsync(TravelInsurance insurance, CancellationToken cancellationToken = default);
    Task DeleteAsync(string companyCode, long planNumber, CancellationToken cancellationToken = default);
}
