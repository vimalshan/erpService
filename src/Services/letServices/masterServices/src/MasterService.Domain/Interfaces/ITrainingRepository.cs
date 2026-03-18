using MasterService.Domain.Entities;

namespace MasterService.Domain.Interfaces;

public interface ITrainingRepository
{
    Task<TrainingProvider?> GetByCodeAsync(long trainingCode, CancellationToken ct = default);
    Task<IEnumerable<TrainingProvider>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(TrainingProvider provider, CancellationToken ct = default);
    Task UpdateAsync(TrainingProvider provider, CancellationToken ct = default);
}
