using FilingAndArchiveService.Domain.Entities;

namespace FilingAndArchiveService.Domain.Interfaces;

public interface IFilingCounterRepository
{
    Task<FilingCounter?> GetByBuIdAsync(string buId, CancellationToken cancellationToken = default);
    Task<long> GetNextCountAsync(string buId, CancellationToken cancellationToken = default);
    Task UpsertAsync(FilingCounter counter, CancellationToken cancellationToken = default);
}
