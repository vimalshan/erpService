using CSA.Service.Domain.Entities;
using CSA.Service.Domain.Interfaces;
using CSA.Service.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CSA.Service.Infrastructure.Repositories;

public class ControlRepository(CsaDbContext context) : IControlRepository
{
    public async Task<Control?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Controls.Include(c => c.Evidences).FirstOrDefaultAsync(c => c.ControlId == id, ct);

    public async Task<IEnumerable<Control>> GetAllAsync(CancellationToken ct = default) =>
        await context.Controls.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<Control>> GetByProcessIdAsync(long processId, CancellationToken ct = default) =>
        await context.Controls.AsNoTracking().Where(c => c.ProcessId == processId).ToListAsync(ct);

    public async Task<Control> AddAsync(Control control, CancellationToken ct = default)
    {
        await context.Controls.AddAsync(control, ct);
        return control;
    }

    public Task UpdateAsync(Control control, CancellationToken ct = default)
    {
        context.Controls.Update(control);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await context.Controls.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Control {id} not found.");
        context.Controls.Remove(entity);
    }
}

public class SurveyRepository(CsaDbContext context) : ISurveyRepository
{
    public async Task<Survey?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Surveys.Include(s => s.Questions).FirstOrDefaultAsync(s => s.SurveyId == id, ct);

    public async Task<IEnumerable<Survey>> GetAllAsync(CancellationToken ct = default) =>
        await context.Surveys.AsNoTracking().ToListAsync(ct);

    public async Task<Survey> AddAsync(Survey survey, CancellationToken ct = default)
    {
        await context.Surveys.AddAsync(survey, ct);
        return survey;
    }

    public Task UpdateAsync(Survey survey, CancellationToken ct = default)
    {
        context.Surveys.Update(survey);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await context.Surveys.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Survey {id} not found.");
        context.Surveys.Remove(entity);
    }
}

public class SurveyQuestionRepository(CsaDbContext context) : ISurveyQuestionRepository
{
    public async Task<SurveyQuestion?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.SurveyQuestions.Include(q => q.Feedbacks).FirstOrDefaultAsync(q => q.SurveyQuestionId == id, ct);

    public async Task<IEnumerable<SurveyQuestion>> GetBySurveyIdAsync(long surveyId, CancellationToken ct = default) =>
        await context.SurveyQuestions.AsNoTracking().Where(q => q.SurveyId == surveyId).ToListAsync(ct);

    public async Task<SurveyQuestion> AddAsync(SurveyQuestion question, CancellationToken ct = default)
    {
        await context.SurveyQuestions.AddAsync(question, ct);
        return question;
    }

    public Task UpdateAsync(SurveyQuestion question, CancellationToken ct = default)
    {
        context.SurveyQuestions.Update(question);
        return Task.CompletedTask;
    }
}

public class SurveyFeedbackRepository(CsaDbContext context) : ISurveyFeedbackRepository
{
    public async Task<SurveyFeedback?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.SurveyFeedbacks.FirstOrDefaultAsync(f => f.FeedbackId == id, ct);

    public async Task<IEnumerable<SurveyFeedback>> GetByQuestionIdAsync(long questionId, CancellationToken ct = default) =>
        await context.SurveyFeedbacks.AsNoTracking().Where(f => f.SurveyQuestionId == questionId).ToListAsync(ct);

    public async Task<SurveyFeedback> AddAsync(SurveyFeedback feedback, CancellationToken ct = default)
    {
        await context.SurveyFeedbacks.AddAsync(feedback, ct);
        return feedback;
    }

    public Task UpdateAsync(SurveyFeedback feedback, CancellationToken ct = default)
    {
        context.SurveyFeedbacks.Update(feedback);
        return Task.CompletedTask;
    }
}

public class ProcessRepository(CsaDbContext context) : IProcessRepository
{
    public async Task<Process?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Processes.Include(p => p.SubProcesses).FirstOrDefaultAsync(p => p.ProcessId == id, ct);

    public async Task<IEnumerable<Process>> GetAllAsync(CancellationToken ct = default) =>
        await context.Processes.AsNoTracking().ToListAsync(ct);

    public async Task<Process> AddAsync(Process process, CancellationToken ct = default)
    {
        await context.Processes.AddAsync(process, ct);
        return process;
    }

    public Task UpdateAsync(Process process, CancellationToken ct = default)
    {
        context.Processes.Update(process);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await context.Processes.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Process {id} not found.");
        context.Processes.Remove(entity);
    }
}

public class SubProcessRepository(CsaDbContext context) : ISubProcessRepository
{
    public async Task<SubProcess?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.SubProcesses.FirstOrDefaultAsync(sp => sp.SubProcessId == id, ct);

    public async Task<IEnumerable<SubProcess>> GetByProcessIdAsync(long processId, CancellationToken ct = default) =>
        await context.SubProcesses.AsNoTracking().Where(sp => sp.ProcessId == processId).ToListAsync(ct);

    public async Task<SubProcess> AddAsync(SubProcess subProcess, CancellationToken ct = default)
    {
        await context.SubProcesses.AddAsync(subProcess, ct);
        return subProcess;
    }

    public Task UpdateAsync(SubProcess subProcess, CancellationToken ct = default)
    {
        context.SubProcesses.Update(subProcess);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await context.SubProcesses.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"SubProcess {id} not found.");
        context.SubProcesses.Remove(entity);
    }
}

public class UnitRepository(CsaDbContext context) : IUnitRepository
{
    public async Task<Unit?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Units.FirstOrDefaultAsync(u => u.UnitId == id, ct);

    public async Task<IEnumerable<Unit>> GetAllAsync(CancellationToken ct = default) =>
        await context.Units.AsNoTracking().ToListAsync(ct);

    public async Task<Unit> AddAsync(Unit unit, CancellationToken ct = default)
    {
        await context.Units.AddAsync(unit, ct);
        return unit;
    }

    public Task UpdateAsync(Unit unit, CancellationToken ct = default)
    {
        context.Units.Update(unit);
        return Task.CompletedTask;
    }
}

public class EvidenceRepository(CsaDbContext context) : IEvidenceRepository
{
    public async Task<Evidence?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.Evidences.FirstOrDefaultAsync(e => e.EvidenceId == id, ct);

    public async Task<IEnumerable<Evidence>> GetByControlIdAsync(long controlId, CancellationToken ct = default) =>
        await context.Evidences.AsNoTracking().Where(e => e.ControlId == controlId).ToListAsync(ct);

    public async Task<Evidence> AddAsync(Evidence evidence, CancellationToken ct = default)
    {
        await context.Evidences.AddAsync(evidence, ct);
        return evidence;
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await context.Evidences.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Evidence {id} not found.");
        context.Evidences.Remove(entity);
    }
}

public class UnitMapDetailRepository(CsaDbContext context) : IUnitMapDetailRepository
{
    public async Task<UnitMapDetail?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.UnitMapDetails.FirstOrDefaultAsync(m => m.MapId == id, ct);

    public async Task<IEnumerable<UnitMapDetail>> GetByControlIdAsync(long controlId, CancellationToken ct = default) =>
        await context.UnitMapDetails.AsNoTracking().Where(m => m.ControlId == controlId).ToListAsync(ct);

    public async Task<UnitMapDetail> AddAsync(UnitMapDetail mapping, CancellationToken ct = default)
    {
        await context.UnitMapDetails.AddAsync(mapping, ct);
        return mapping;
    }

    public Task UpdateAsync(UnitMapDetail mapping, CancellationToken ct = default)
    {
        context.UnitMapDetails.Update(mapping);
        return Task.CompletedTask;
    }
}
