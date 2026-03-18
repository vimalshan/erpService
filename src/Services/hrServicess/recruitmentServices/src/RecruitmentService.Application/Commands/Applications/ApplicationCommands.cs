using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.Exceptions;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Domain.ValueObjects;
using RecruitmentService.Application.Interfaces;

namespace RecruitmentService.Application.Commands.Applications;

// ── Submit Application ────────────────────────────────────────────────────────

public record SubmitApplicationCommand(SubmitApplicationRequest Request, decimal SubmittedBy) : IRequest<decimal>;

public class SubmitApplicationCommandHandler : IRequestHandler<SubmitApplicationCommand, decimal>
{
    private readonly IApplicationRepository _appRepo;
    private readonly IVacancyRepository _vacancyRepo;
    private readonly IUnitOfWork _uow;

    public SubmitApplicationCommandHandler(
        IApplicationRepository appRepo, IVacancyRepository vacancyRepo, IUnitOfWork uow)
    {
        _appRepo = appRepo;
        _vacancyRepo = vacancyRepo;
        _uow = uow;
    }

    public async Task<decimal> Handle(SubmitApplicationCommand cmd, CancellationToken ct)
    {
        var req = cmd.Request;
        var vacancy = await _vacancyRepo.GetByIdAsync(req.VacancyId, ct)
            ?? throw new VacancyNotFoundException(req.VacancyId);

        if (vacancy.LiveStatus == VacancyStatus.Closed)
            throw new VacancyClosedException(req.VacancyId);

        var application = ApplicationHistory.Submit(req.AppId, req.AppSl, req.AppUnit, req.VacancyId, cmd.SubmittedBy);

        if (req.Qualifications != null)
            foreach (var q in req.Qualifications)
                application.AddQualification(ApplicationQualification.Create(
                    req.AppId, q.QualId, q.QualCode, q.QualDescription,
                    q.YearFrom, q.YearTo, q.InstitutionCode, q.InstitutionDescription,
                    q.EducationType, q.SpecializationCode, q.SpecializationDescription,
                    q.Percentage, q.DegreeCode, q.DegreeDescription, q.InstitutionOthers));

        if (req.Trainings != null)
            foreach (var t in req.Trainings)
                application.AddTraining(ApplicationTraining.Create(
                    req.AppId, t.TrainingId, t.Title, t.Duration, t.Institute, t.Location));

        await _appRepo.AddAsync(application, ct);
        await _uow.SaveChangesAsync(ct);
        return application.AppId;
    }
}

// ── Update Status ─────────────────────────────────────────────────────────────

public record UpdateApplicationStatusCommand(decimal AppId, string StatusCode, string? Remarks, decimal UpdatedBy) : IRequest;

public class UpdateApplicationStatusCommandHandler : IRequestHandler<UpdateApplicationStatusCommand>
{
    private readonly IApplicationRepository _repository;
    private readonly IUnitOfWork _uow;

    public UpdateApplicationStatusCommandHandler(IApplicationRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(UpdateApplicationStatusCommand cmd, CancellationToken ct)
    {
        var app = await _repository.GetByIdAsync(cmd.AppId, ct)
            ?? throw new ApplicationNotFoundException(cmd.AppId);

        var newStatus = ApplicationStatusExtensions.FromCode(cmd.StatusCode);
        app.UpdateStatus(newStatus, cmd.Remarks, cmd.UpdatedBy);
        _repository.Update(app);
        await _uow.SaveChangesAsync(ct);
    }
}
