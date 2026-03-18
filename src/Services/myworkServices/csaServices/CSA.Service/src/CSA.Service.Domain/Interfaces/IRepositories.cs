using CSA.Service.Domain.Entities;

namespace CSA.Service.Domain.Interfaces;

public interface IControlRepository
{
    Task<Control?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Control>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Control>> GetByProcessIdAsync(long processId, CancellationToken ct = default);
    Task<Control> AddAsync(Control control, CancellationToken ct = default);
    Task UpdateAsync(Control control, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface ISurveyRepository
{
    Task<Survey?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Survey>> GetAllAsync(CancellationToken ct = default);
    Task<Survey> AddAsync(Survey survey, CancellationToken ct = default);
    Task UpdateAsync(Survey survey, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface ISurveyQuestionRepository
{
    Task<SurveyQuestion?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<SurveyQuestion>> GetBySurveyIdAsync(long surveyId, CancellationToken ct = default);
    Task<SurveyQuestion> AddAsync(SurveyQuestion question, CancellationToken ct = default);
    Task UpdateAsync(SurveyQuestion question, CancellationToken ct = default);
}

public interface ISurveyFeedbackRepository
{
    Task<SurveyFeedback?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<SurveyFeedback>> GetByQuestionIdAsync(long questionId, CancellationToken ct = default);
    Task<SurveyFeedback> AddAsync(SurveyFeedback feedback, CancellationToken ct = default);
    Task UpdateAsync(SurveyFeedback feedback, CancellationToken ct = default);
}

public interface IProcessRepository
{
    Task<Process?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Process>> GetAllAsync(CancellationToken ct = default);
    Task<Process> AddAsync(Process process, CancellationToken ct = default);
    Task UpdateAsync(Process process, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface ISubProcessRepository
{
    Task<SubProcess?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<SubProcess>> GetByProcessIdAsync(long processId, CancellationToken ct = default);
    Task<SubProcess> AddAsync(SubProcess subProcess, CancellationToken ct = default);
    Task UpdateAsync(SubProcess subProcess, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IUnitRepository
{
    Task<Unit?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Unit>> GetAllAsync(CancellationToken ct = default);
    Task<Unit> AddAsync(Unit unit, CancellationToken ct = default);
    Task UpdateAsync(Unit unit, CancellationToken ct = default);
}

public interface IEvidenceRepository
{
    Task<Evidence?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<Evidence>> GetByControlIdAsync(long controlId, CancellationToken ct = default);
    Task<Evidence> AddAsync(Evidence evidence, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IUnitMapDetailRepository
{
    Task<UnitMapDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<UnitMapDetail>> GetByControlIdAsync(long controlId, CancellationToken ct = default);
    Task<UnitMapDetail> AddAsync(UnitMapDetail mapping, CancellationToken ct = default);
    Task UpdateAsync(UnitMapDetail mapping, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
