using DealTicketing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealTicketing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Bank> Banks { get; }
    DbSet<CategoryMaster> CategoryMasters { get; }
    DbSet<LovMaster> LovMasters { get; }
    DbSet<DealBatch> DealBatches { get; }
    DbSet<DealDetail> DealDetails { get; }
    DbSet<DealLoanSchedule> DealLoanSchedules { get; }
    DbSet<DealSettlement> DealSettlements { get; }
    DbSet<DealAttachment> DealAttachments { get; }
    DbSet<DealSettlementAttachment> DealSettlementAttachments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<string> GetSasUriAsync(string containerName, string blobName, TimeSpan expiry, CancellationToken ct = default);
}

public interface ICurrentUserService
{
    long? UserId { get; }
    string? UserName { get; }
    IEnumerable<string> Roles { get; }
}
