using LovService.Domain.Entities;

namespace LovService.Domain.Interfaces;

public interface IUnitOfWork
{
    ILovTypeMastRepository LovTypeMasts { get; }
    ILovMasterRepository LovMasters { get; }
    IProgramLovMastRepository ProgramLovMasts { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
