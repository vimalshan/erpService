using EximManagement.Domain.Entities;

namespace EximManagement.Application.Interfaces;

public interface IEximDataFileRepository
{
    Task<EximDataFile?> GetByIdAsync(long fileId, CancellationToken ct = default);
    Task<IEnumerable<EximDataFile>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(EximDataFile file, CancellationToken ct = default);
    Task UpdateAsync(EximDataFile file, CancellationToken ct = default);
    Task<IEnumerable<EximDataFile>> GetByTypeAsync(string fileType, CancellationToken ct = default);
}

public interface IEximProductRepository
{
    Task<EximProduct?> GetByIdAsync(long productId, CancellationToken ct = default);
    Task<IEnumerable<EximProduct>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(EximProduct product, CancellationToken ct = default);
    Task UpdateAsync(EximProduct product, CancellationToken ct = default);
    Task<EximProduct?> GetByNameAsync(string productName, CancellationToken ct = default);
}

public interface IEximProductGroupRepository
{
    Task<EximProductGroup?> GetByIdAsync(long groupId, CancellationToken ct = default);
    Task<IEnumerable<EximProductGroup>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(EximProductGroup group, CancellationToken ct = default);
    Task UpdateAsync(EximProductGroup group, CancellationToken ct = default);
}

public interface IEximDataExportRepository
{
    Task<EximDataExport?> GetByIdAsync(long dataId, CancellationToken ct = default);
    Task<IEnumerable<EximDataExport>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IEnumerable<EximDataExport>> GetByFileIdAsync(long fileId, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<EximDataExport> records, CancellationToken ct = default);
}

public interface IEximDataImportRepository
{
    Task<EximDataImport?> GetByIdAsync(long dataId, CancellationToken ct = default);
    Task<IEnumerable<EximDataImport>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IEnumerable<EximDataImport>> GetByFileIdAsync(long fileId, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<EximDataImport> records, CancellationToken ct = default);
}
