using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Domain.Interfaces;

public interface IProgramLovRepository
{
    Task<ProgramLovMaster?> GetByTypeCodeAsync(string typeCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProgramLovMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ProgramLovMaster entity, CancellationToken cancellationToken = default);
    void Update(ProgramLovMaster entity);
    void Delete(ProgramLovMaster entity);
}
