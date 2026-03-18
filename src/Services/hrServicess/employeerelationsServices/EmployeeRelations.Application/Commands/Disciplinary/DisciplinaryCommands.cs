using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Aggregates;
using EmployeeRelations.Domain.Exceptions;
using FluentValidation;
using AutoMapper;

namespace EmployeeRelations.Application.Commands.Disciplinary;

// ---- Create Disciplinary Case ----
public record CreateDisciplinaryCaseCommand(long UnitId, DateTime Date, string Details, long CreatedBy, IEnumerable<long> EmployeeIds) : IRequest<DisciplinaryMainDto>;

public class CreateDisciplinaryCaseValidator : AbstractValidator<CreateDisciplinaryCaseCommand>
{
    public CreateDisciplinaryCaseValidator()
    {
        RuleFor(x => x.UnitId).GreaterThan(0);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Details).NotEmpty().MaximumLength(500);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateDisciplinaryCaseHandler : IRequestHandler<CreateDisciplinaryCaseCommand, DisciplinaryMainDto>
{
    private readonly IDisciplinaryRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateDisciplinaryCaseHandler(IDisciplinaryRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<DisciplinaryMainDto> Handle(CreateDisciplinaryCaseCommand req, CancellationToken ct)
    {
        var id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var discipline = new DisciplinaryMain(id, req.UnitId, req.Date, req.Details, req.CreatedBy);
        foreach (var empId in req.EmployeeIds)
            discipline.AddEmployee(empId);
        await _repo.AddAsync(discipline, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<DisciplinaryMainDto>(discipline);
    }
}

// ---- Add Disciplinary Action ----
public record AddDisciplinaryActionCommand(long MainId, long EmpSysId, long TypeId, DateTime ActionDate, string Remarks, long CreatedBy) : IRequest<DisciplinaryActionDto>;

public class AddDisciplinaryActionValidator : AbstractValidator<AddDisciplinaryActionCommand>
{
    public AddDisciplinaryActionValidator()
    {
        RuleFor(x => x.MainId).GreaterThan(0);
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.TypeId).GreaterThan(0);
        RuleFor(x => x.Remarks).NotEmpty().MaximumLength(500);
    }
}

public class AddDisciplinaryActionHandler : IRequestHandler<AddDisciplinaryActionCommand, DisciplinaryActionDto>
{
    private readonly IDisciplinaryRepository _repo;
    private readonly IUnitOfWork _uow;

    public AddDisciplinaryActionHandler(IDisciplinaryRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DisciplinaryActionDto> Handle(AddDisciplinaryActionCommand req, CancellationToken ct)
    {
        var discipline = await _repo.GetByIdAsync(req.MainId, ct)
            ?? throw new EntityNotFoundException(nameof(DisciplinaryMain), req.MainId);
        var actionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        discipline.AddAction(actionId, req.EmpSysId, req.TypeId, req.ActionDate, req.Remarks, req.CreatedBy);
        await _repo.UpdateAsync(discipline, ct);
        await _uow.SaveChangesAsync(ct);
        var action = discipline.Actions.Last();
        return new DisciplinaryActionDto(action.ActionId, action.MainId, action.EmpSysId, action.TypeId, action.ActionDate, action.Remarks, action.DocPath, action.EntryStatus);
    }
}

// ---- Approve Disciplinary Action ----
public record ApproveDisciplinaryActionCommand(long MainId, long ActionId, long ApprovedBy) : IRequest<Unit>;

public class ApproveDisciplinaryActionHandler : IRequestHandler<ApproveDisciplinaryActionCommand, Unit>
{
    private readonly IDisciplinaryRepository _repo;
    private readonly IUnitOfWork _uow;

    public ApproveDisciplinaryActionHandler(IDisciplinaryRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(ApproveDisciplinaryActionCommand req, CancellationToken ct)
    {
        var discipline = await _repo.GetByIdAsync(req.MainId, ct)
            ?? throw new EntityNotFoundException(nameof(DisciplinaryMain), req.MainId);
        var action = discipline.Actions.FirstOrDefault(a => a.ActionId == req.ActionId)
            ?? throw new EntityNotFoundException(nameof(DisciplinaryAction), req.ActionId);
        action.Approve(req.ApprovedBy);
        await _repo.UpdateAsync(discipline, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
