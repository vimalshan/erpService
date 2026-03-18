using CompensationBenefits.Domain.Entities;
using CompensationBenefits.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Application.Features.Salaries.Commands;

// ─── Create ─────────────────────────────────────────────────────────────────────
public record CreateSalaryCommand(
    long SalaryId,
    string SalaryType,
    decimal SalaryCTC,
    long SalaryStructureId,
    long SalaryFooterId,
    long CreatedBy) : IRequest<long>;

public class CreateSalaryCommandValidator : AbstractValidator<CreateSalaryCommand>
{
    public CreateSalaryCommandValidator()
    {
        RuleFor(x => x.SalaryId).GreaterThan(0);
        RuleFor(x => x.SalaryType).NotEmpty().MaximumLength(1).Must(t => t is "C" or "F")
            .WithMessage("SalaryType must be C (CTC Based) or F (Fixed).");
        RuleFor(x => x.SalaryCTC).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalaryStructureId).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateSalaryCommandHandler(ISalaryRepository repo)
    : IRequestHandler<CreateSalaryCommand, long>
{
    public async Task<long> Handle(CreateSalaryCommand request, CancellationToken ct)
    {
        var salary = SalaryMain.Create(
            request.SalaryId, request.SalaryType, request.SalaryCTC,
            request.SalaryStructureId, request.SalaryFooterId, request.CreatedBy);

        await repo.AddAsync(salary, ct);
        await repo.SaveChangesAsync(ct);
        return salary.SalaryId;
    }
}

// ─── Cancel ─────────────────────────────────────────────────────────────────────
public record CancelSalaryCommand(long SalaryId, long CancelledBy) : IRequest<bool>;

public class CancelSalaryCommandHandler(ISalaryRepository repo)
    : IRequestHandler<CancelSalaryCommand, bool>
{
    public async Task<bool> Handle(CancelSalaryCommand request, CancellationToken ct)
    {
        var salary = await repo.GetByIdAsync(request.SalaryId, ct);
        if (salary is null) return false;

        salary.Cancel(request.CancelledBy);
        repo.Update(salary);
        await repo.SaveChangesAsync(ct);
        return true;
    }
}

// ─── Process Pending Salaries (Background / Azure Function) ──────────────────────
public record ProcessPendingSalariesCommand : IRequest<int>;

public class ProcessPendingSalariesCommandHandler(ISalaryRepository repo, Microsoft.Extensions.Logging.ILogger<ProcessPendingSalariesCommandHandler> logger)
    : IRequestHandler<ProcessPendingSalariesCommand, int>
{
    public async Task<int> Handle(ProcessPendingSalariesCommand request, CancellationToken ct)
    {
        // Retrieve all active (non-cancelled) salary records for batch processing
        var all = await repo.GetAllAsync(ct);
        var pending = all.Where(s => s.SalaryCancelledOn == null).ToList();

        logger.LogInformation("Processing {Count} pending salary records.", pending.Count);

        // Domain logic / downstream calls would go here

        return pending.Count;
    }
}
