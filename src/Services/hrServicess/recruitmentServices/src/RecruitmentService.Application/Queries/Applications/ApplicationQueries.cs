using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Domain.Exceptions;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Application.Queries.Applications;

// ── Get By ID ─────────────────────────────────────────────────────────────────

public record GetApplicationByIdQuery(decimal AppId) : IRequest<ApplicationDto>;

public class GetApplicationByIdQueryHandler : IRequestHandler<GetApplicationByIdQuery, ApplicationDto>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationByIdQueryHandler(IApplicationRepository repository) => _repository = repository;

    public async Task<ApplicationDto> Handle(GetApplicationByIdQuery query, CancellationToken ct)
    {
        var a = await _repository.GetByIdAsync(query.AppId, ct)
            ?? throw new ApplicationNotFoundException(query.AppId);

        return MapToDto(a);
    }

    internal static ApplicationDto MapToDto(Domain.Entities.ApplicationHistory a) => new(
        a.AppId, a.AppSl, a.AppUnit, a.AppVacancyId, a.Status.ToCode(),
        a.Remarks, a.UpdatedBy, a.UpdatedOn,
        a.Qualifications.Select(q => new ApplicationQualificationDto(
            q.AppId, q.AppQualId, q.QualCode, q.QualDescription, q.YearFrom, q.YearTo,
            q.InstitutionCode, q.InstitutionDescription, q.EducationType,
            q.SpecializationCode, q.SpecializationDescription, q.Percentage,
            q.DegreeCode, q.DegreeDescription, q.InstitutionOthers)),
        a.Trainings.Select(t => new ApplicationTrainingDto(
            t.AppId, t.TrainingId, t.Title, t.Duration, t.Institute, t.Location)));
}

// ── Get By Vacancy ────────────────────────────────────────────────────────────

public record GetApplicationsByVacancyQuery(decimal VacancyId) : IRequest<IEnumerable<ApplicationDto>>;

public class GetApplicationsByVacancyQueryHandler : IRequestHandler<GetApplicationsByVacancyQuery, IEnumerable<ApplicationDto>>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationsByVacancyQueryHandler(IApplicationRepository repository) => _repository = repository;

    public async Task<IEnumerable<ApplicationDto>> Handle(GetApplicationsByVacancyQuery query, CancellationToken ct)
    {
        var apps = await _repository.GetByVacancyIdAsync(query.VacancyId, ct);
        return apps.Select(GetApplicationByIdQueryHandler.MapToDto);
    }
}
