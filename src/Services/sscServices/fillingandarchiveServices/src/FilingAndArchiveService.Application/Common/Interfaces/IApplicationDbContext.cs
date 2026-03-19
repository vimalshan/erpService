using FilingAndArchiveService.Domain.Entities;

namespace FilingAndArchiveService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<FileMaster> FileMasters { get; }
    DbSet<FilingCounter> FilingCounters { get; }
    DbSet<FilingDocPrint> FilingDocPrints { get; }
    DbSet<FilingDocErrorList> FilingDocErrorLists { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
