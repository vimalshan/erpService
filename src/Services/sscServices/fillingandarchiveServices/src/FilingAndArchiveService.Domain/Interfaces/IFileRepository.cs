using FilingAndArchiveService.Domain.Entities;

namespace FilingAndArchiveService.Domain.Interfaces;

public interface IFileRepository
{
    Task<FileMaster?> GetByIdAsync(long fileId, CancellationToken cancellationToken = default);
    Task<FileMaster?> GetByFileNoAsync(string orgId, string fileNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<FileMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FileMaster>> GetByOrgAsync(string orgId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FileMaster>> GetByYearAsync(long year, CancellationToken cancellationToken = default);
    Task AddAsync(FileMaster file, CancellationToken cancellationToken = default);
    Task UpdateAsync(FileMaster file, CancellationToken cancellationToken = default);
    Task DeleteAsync(long fileId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long fileId, CancellationToken cancellationToken = default);
}
