using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Interfaces;

public interface IFillingPlantRepository
{
    Task<FillingPlant?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FillingPlant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FillingPlant>> GetByCompanyUnitAsync(int companyUnitId, CancellationToken cancellationToken = default);
    Task AddAsync(FillingPlant plant, CancellationToken cancellationToken = default);
    void Update(FillingPlant plant);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
