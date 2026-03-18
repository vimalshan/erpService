using LovService.Domain.Entities;

namespace LovService.Domain.Interfaces;

public interface IProgramLovMastRepository
{
    Task<ProgramLovMast?> GetByIdAsync(string prlovTypeCode, string prlovCode, CancellationToken ct = default);
    Task<IEnumerable<ProgramLovMast>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ProgramLovMast>> GetByTypeCodeAsync(string prlovTypeCode, CancellationToken ct = default);
    Task AddAsync(ProgramLovMast entity, CancellationToken ct = default);
    Task UpdateAsync(ProgramLovMast entity, CancellationToken ct = default);
    Task DeleteAsync(string prlovTypeCode, string prlovCode, CancellationToken ct = default);
    Task<bool> ExistsAsync(string prlovTypeCode, string prlovCode, CancellationToken ct = default);
}
