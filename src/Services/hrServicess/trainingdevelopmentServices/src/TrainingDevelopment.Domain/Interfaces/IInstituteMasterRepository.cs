using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Domain.Interfaces;

public interface IInstituteMasterRepository
{
    Task<InstituteMaster?> GetByCodeAsync(decimal code, CancellationToken cancellationToken = default);
    Task<IEnumerable<InstituteMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(InstituteMaster entity, CancellationToken cancellationToken = default);
    void Update(InstituteMaster entity);
    void Delete(InstituteMaster entity);
}
