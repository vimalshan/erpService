using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Domain.Interfaces;

public interface ITrainingDetailRepository
{
    Task<TrainingDetail?> GetByIdAsync(decimal id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrainingDetail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TrainingDetail>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrainingDetail>> GetByFinancialYearAsync(decimal year, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrainingDetail>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task AddAsync(TrainingDetail entity, CancellationToken cancellationToken = default);
    void Update(TrainingDetail entity);
    void Delete(TrainingDetail entity);
}
