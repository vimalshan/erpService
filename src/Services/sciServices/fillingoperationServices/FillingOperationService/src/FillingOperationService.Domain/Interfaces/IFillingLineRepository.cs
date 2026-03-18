using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Interfaces;

public interface IFillingLineRepository
{
    Task<FillingLine?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FillingLine>> GetByPlantIdAsync(int plantId, CancellationToken cancellationToken = default);
    Task AddAsync(FillingLine line, CancellationToken cancellationToken = default);
    void Update(FillingLine line);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
