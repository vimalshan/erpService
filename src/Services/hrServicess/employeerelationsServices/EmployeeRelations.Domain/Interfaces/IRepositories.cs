using EmployeeRelations.Domain.Aggregates;

namespace EmployeeRelations.Domain.Interfaces;

public interface IDisciplinaryRepository
{
    Task<DisciplinaryMain?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<DisciplinaryMain>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(DisciplinaryMain entity, CancellationToken ct = default);
    Task UpdateAsync(DisciplinaryMain entity, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
    Task<bool> ExistsAsync(long id, CancellationToken ct = default);
}

public interface IEwsRepository
{
    Task<EwsMain?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EwsMain>> GetByEmpAsync(long empSysId, CancellationToken ct = default);
    Task<IEnumerable<EwsMain>> GetByPeriodAsync(int periodNo, CancellationToken ct = default);
    Task AddAsync(EwsMain entity, CancellationToken ct = default);
    Task UpdateAsync(EwsMain entity, CancellationToken ct = default);
}

public interface ISurveyRepository
{
    Task<SurveyMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<SurveyMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SurveyMaster entity, CancellationToken ct = default);
    Task UpdateAsync(SurveyMaster entity, CancellationToken ct = default);
    Task<SurveyResponseMain?> GetResponseAsync(long responseId, CancellationToken ct = default);
    Task AddResponseAsync(SurveyResponseMain entity, CancellationToken ct = default);
}
