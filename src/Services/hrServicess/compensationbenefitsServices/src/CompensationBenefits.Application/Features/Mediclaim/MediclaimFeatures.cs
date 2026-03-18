using AutoMapper;
using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Domain.Entities;
using CompensationBenefits.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Application.Features.Mediclaim;

// ─── Queries ────────────────────────────────────────────────────────────────────
public record GetMediclaimByIdQuery(long MediclaimId) : IRequest<MediclaimDto?>;

public class GetMediclaimByIdQueryHandler(IMediclaimRepository repo, IMapper mapper)
    : IRequestHandler<GetMediclaimByIdQuery, MediclaimDto?>
{
    public async Task<MediclaimDto?> Handle(GetMediclaimByIdQuery request, CancellationToken ct)
    {
        var m = await repo.GetWithDetailsAsync(request.MediclaimId, ct);
        return m is null ? null : mapper.Map<MediclaimDto>(m);
    }
}

public record GetAllMediclainsQuery : IRequest<IEnumerable<MediclaimDto>>;

public class GetAllMediclainsQueryHandler(IMediclaimRepository repo, IMapper mapper)
    : IRequestHandler<GetAllMediclainsQuery, IEnumerable<MediclaimDto>>
{
    public async Task<IEnumerable<MediclaimDto>> Handle(GetAllMediclainsQuery request, CancellationToken ct)
        => mapper.Map<IEnumerable<MediclaimDto>>(await repo.GetAllAsync(ct));
}

// ─── Commands ───────────────────────────────────────────────────────────────────
public record CreateMediclaimCommand(
    long MediclaimId,
    string RefName,
    string Type,
    string PaidBy,
    DateTime StartDate,
    DateTime CloseDate) : IRequest<long>;

public class CreateMediclaimCommandValidator : AbstractValidator<CreateMediclaimCommand>
{
    public CreateMediclaimCommandValidator()
    {
        RuleFor(x => x.MediclaimId).GreaterThan(0);
        RuleFor(x => x.RefName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).Must(t => t is "I" or "F").WithMessage("Type must be I (Individual) or F (Family).");
        RuleFor(x => x.PaidBy).Must(p => p is "E" or "C" or "U").WithMessage("PaidBy must be E, C, or U.");
        RuleFor(x => x.CloseDate).GreaterThan(x => x.StartDate);
    }
}

public class CreateMediclaimCommandHandler(IMediclaimRepository repo)
    : IRequestHandler<CreateMediclaimCommand, long>
{
    public async Task<long> Handle(CreateMediclaimCommand request, CancellationToken ct)
    {
        var m = MediclaimMaster.Create(
            request.MediclaimId, request.RefName, request.Type,
            request.PaidBy, request.StartDate, request.CloseDate);

        await repo.AddAsync(m, ct);
        await repo.SaveChangesAsync(ct);
        return m.MediclaimId;
    }
}

// ─── Check Renewals (Background / Azure Function) ─────────────────────────────
public record CheckMediclaimRenewalsCommand : IRequest<int>;

public class CheckMediclaimRenewalsCommandHandler(IMediclaimRepository repo, Microsoft.Extensions.Logging.ILogger<CheckMediclaimRenewalsCommandHandler> logger)
    : IRequestHandler<CheckMediclaimRenewalsCommand, int>
{
    public async Task<int> Handle(CheckMediclaimRenewalsCommand request, CancellationToken ct)
    {
        var allMediclaims = await repo.GetAllAsync(ct);
        var dueSoon = allMediclaims
            .Where(m => m.MediclaimCloseDate.HasValue
                     && m.MediclaimCloseDate.Value >= DateTime.UtcNow
                     && m.MediclaimCloseDate.Value <= DateTime.UtcNow.AddDays(30))
            .ToList();

        logger.LogInformation("Found {Count} mediclaim records due for renewal within 30 days.", dueSoon.Count);

        // In production: send renewal notifications, trigger approval workflows, etc.

        return dueSoon.Count;
    }
}
