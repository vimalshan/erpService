using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Aggregates;
using EmployeeRelations.Domain.ValueObjects;
using EmployeeRelations.Domain.Exceptions;
using FluentValidation;
using AutoMapper;

namespace EmployeeRelations.Application.Commands.Ews;

// ---- Create EWS ----
public record CreateEwsCommand(long EmpSysId, int PeriodNo) : IRequest<EwsMainDto>;

public class CreateEwsValidator : AbstractValidator<CreateEwsCommand>
{
    public CreateEwsValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.PeriodNo).GreaterThan(0);
    }
}

public class CreateEwsHandler : IRequestHandler<CreateEwsCommand, EwsMainDto>
{
    private readonly IEwsRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateEwsHandler(IEwsRepository repo, IUnitOfWork uow, IMapper mapper) { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<EwsMainDto> Handle(CreateEwsCommand req, CancellationToken ct)
    {
        var id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var ews = new EwsMain(id, req.EmpSysId, req.PeriodNo);
        await _repo.AddAsync(ews, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<EwsMainDto>(ews);
    }
}

// ---- Record HR Input ----
public record RecordEwsHrInputCommand(long EwsId, long HrEntryBy, string HrFlag, string? HrRemarks) : IRequest<Unit>;

public class RecordEwsHrInputValidator : AbstractValidator<RecordEwsHrInputCommand>
{
    public RecordEwsHrInputValidator()
    {
        RuleFor(x => x.EwsId).GreaterThan(0);
        RuleFor(x => x.HrEntryBy).GreaterThan(0);
        RuleFor(x => x.HrFlag).NotEmpty().Must(f => f == "R" || f == "G" || f == "A")
            .WithMessage("HrFlag must be R, G, or A");
    }
}

public class RecordEwsHrInputHandler : IRequestHandler<RecordEwsHrInputCommand, Unit>
{
    private readonly IEwsRepository _repo;
    private readonly IUnitOfWork _uow;

    public RecordEwsHrInputHandler(IEwsRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(RecordEwsHrInputCommand req, CancellationToken ct)
    {
        var ews = await _repo.GetByIdAsync(req.EwsId, ct)
            ?? throw new EntityNotFoundException(nameof(EwsMain), req.EwsId);
        ews.RecordHrInput(req.HrEntryBy, EwsFlag.From(req.HrFlag), req.HrRemarks);
        await _repo.UpdateAsync(ews, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
