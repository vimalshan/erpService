using AutoMapper;
using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Domain.Entities;
using CompensationBenefits.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CompensationBenefits.Application.Features.SalaryStructures;

// ─── Queries ────────────────────────────────────────────────────────────────────
public record GetSalaryStructureByIdQuery(long StructureId) : IRequest<SalaryStructureDto?>;

public class GetSalaryStructureByIdQueryHandler(ISalaryStructureRepository repo, IMapper mapper)
    : IRequestHandler<GetSalaryStructureByIdQuery, SalaryStructureDto?>
{
    public async Task<SalaryStructureDto?> Handle(GetSalaryStructureByIdQuery request, CancellationToken ct)
    {
        var s = await repo.GetWithDetailsAsync(request.StructureId, ct);
        return s is null ? null : mapper.Map<SalaryStructureDto>(s);
    }
}

public record GetAllSalaryStructuresQuery : IRequest<IEnumerable<SalaryStructureDto>>;

public class GetAllSalaryStructuresQueryHandler(ISalaryStructureRepository repo, IMapper mapper)
    : IRequestHandler<GetAllSalaryStructuresQuery, IEnumerable<SalaryStructureDto>>
{
    public async Task<IEnumerable<SalaryStructureDto>> Handle(GetAllSalaryStructuresQuery request, CancellationToken ct)
        => mapper.Map<IEnumerable<SalaryStructureDto>>(await repo.GetAllAsync(ct));
}

// ─── Commands ───────────────────────────────────────────────────────────────────
public record CreateSalaryStructureCommand(
    long StructureId,
    long UnitId,
    string Name,
    string GradeCategory,
    long GradeId,
    string Type,
    decimal CtcMin,
    decimal CtcMax,
    long FooterId,
    long CreatedBy) : IRequest<long>;

public class CreateSalaryStructureCommandValidator : AbstractValidator<CreateSalaryStructureCommand>
{
    public CreateSalaryStructureCommandValidator()
    {
        RuleFor(x => x.StructureId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).Must(t => t is "C" or "F").WithMessage("Type must be C or F.");
        RuleFor(x => x.CtcMax).GreaterThanOrEqualTo(x => x.CtcMin);
    }
}

public class CreateSalaryStructureCommandHandler(ISalaryStructureRepository repo)
    : IRequestHandler<CreateSalaryStructureCommand, long>
{
    public async Task<long> Handle(CreateSalaryStructureCommand request, CancellationToken ct)
    {
        var s = SalaryStructureMain.Create(
            request.StructureId, request.UnitId, request.Name, request.GradeCategory,
            request.GradeId, request.Type, request.CtcMin, request.CtcMax,
            request.FooterId, request.CreatedBy);

        await repo.AddAsync(s, ct);
        await repo.SaveChangesAsync(ct);
        return s.StructureId;
    }
}

public record UpdateSalaryStructureCommand(
    long StructureId,
    string Name,
    decimal CtcMin,
    decimal CtcMax,
    long ModifiedBy) : IRequest<bool>;

public class UpdateSalaryStructureCommandHandler(ISalaryStructureRepository repo)
    : IRequestHandler<UpdateSalaryStructureCommand, bool>
{
    public async Task<bool> Handle(UpdateSalaryStructureCommand request, CancellationToken ct)
    {
        var s = await repo.GetByIdAsync(request.StructureId, ct);
        if (s is null) return false;

        s.Update(request.Name, request.CtcMin, request.CtcMax, request.ModifiedBy);
        repo.Update(s);
        await repo.SaveChangesAsync(ct);
        return true;
    }
}
