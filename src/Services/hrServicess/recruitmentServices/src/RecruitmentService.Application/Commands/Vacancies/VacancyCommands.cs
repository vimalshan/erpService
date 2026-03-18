using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.Exceptions;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Application.Interfaces;

namespace RecruitmentService.Application.Commands.Vacancies;

// ── Create ────────────────────────────────────────────────────────────────────

public record CreateVacancyCommand(CreateVacancyRequest Request, decimal PostedBy) : IRequest<decimal>;

public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, decimal>
{
    private readonly IVacancyRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreateVacancyCommandHandler(IVacancyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<decimal> Handle(CreateVacancyCommand cmd, CancellationToken ct)
    {
        var req = cmd.Request;
        var vacancy = Vacancy.Create(
            req.VacancyId, req.VacancyUnit, req.VacancyGrade, req.VacancyPositionId,
            req.VacancyName, req.VacancyLocation, req.VacancyProcess,
            req.VacancyAge, req.VacancyExperience, req.VacancyQualification,
            req.VacancyUnitId, cmd.PostedBy);

        await _repository.AddAsync(vacancy, ct);
        await _uow.SaveChangesAsync(ct);
        return vacancy.VacancyId;
    }
}

// ── Update ────────────────────────────────────────────────────────────────────

public record UpdateVacancyCommand(decimal VacancyId, UpdateVacancyRequest Request, decimal ModifiedBy) : IRequest;

public class UpdateVacancyCommandHandler : IRequestHandler<UpdateVacancyCommand>
{
    private readonly IVacancyRepository _repository;
    private readonly IUnitOfWork _uow;

    public UpdateVacancyCommandHandler(IVacancyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(UpdateVacancyCommand cmd, CancellationToken ct)
    {
        var vacancy = await _repository.GetByIdAsync(cmd.VacancyId, ct)
            ?? throw new VacancyNotFoundException(cmd.VacancyId);

        var req = cmd.Request;
        vacancy.UpdateDetails(req.VacancyName, req.VacancyAge, req.VacancyExperience,
            req.VacancyQualification, req.VacancyNarration1, req.VacancyNarration2,
            req.VacancyNarration3, req.VacancyNarration4, req.VacancyLastDate, cmd.ModifiedBy);

        _repository.Update(vacancy);
        await _uow.SaveChangesAsync(ct);
    }
}

// ── Close ─────────────────────────────────────────────────────────────────────

public record CloseVacancyCommand(decimal VacancyId, decimal ModifiedBy) : IRequest;

public class CloseVacancyCommandHandler : IRequestHandler<CloseVacancyCommand>
{
    private readonly IVacancyRepository _repository;
    private readonly IUnitOfWork _uow;

    public CloseVacancyCommandHandler(IVacancyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(CloseVacancyCommand cmd, CancellationToken ct)
    {
        var vacancy = await _repository.GetByIdAsync(cmd.VacancyId, ct)
            ?? throw new VacancyNotFoundException(cmd.VacancyId);

        vacancy.Close(cmd.ModifiedBy);
        _repository.Update(vacancy);
        await _uow.SaveChangesAsync(ct);
    }
}

// ── Update Attachment ─────────────────────────────────────────────────────────

public record UpdateVacancyAttachmentCommand(decimal VacancyId, string FileName) : IRequest;

public class UpdateVacancyAttachmentCommandHandler : IRequestHandler<UpdateVacancyAttachmentCommand>
{
    private readonly IVacancyRepository _repository;
    private readonly IUnitOfWork _uow;

    public UpdateVacancyAttachmentCommandHandler(IVacancyRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(UpdateVacancyAttachmentCommand cmd, CancellationToken ct)
    {
        var vacancy = await _repository.GetByIdAsync(cmd.VacancyId, ct)
            ?? throw new VacancyNotFoundException(cmd.VacancyId);

        vacancy.SetAttachment(cmd.FileName);
        _repository.Update(vacancy);
        await _uow.SaveChangesAsync(ct);
    }
}
