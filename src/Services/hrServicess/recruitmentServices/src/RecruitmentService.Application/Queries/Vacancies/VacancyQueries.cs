using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Domain.Exceptions;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Application.Queries.Vacancies;

// ── Get All Open ──────────────────────────────────────────────────────────────

public record GetAllVacanciesQuery(bool OpenOnly = true) : IRequest<IEnumerable<VacancySummaryDto>>;

public class GetAllVacanciesQueryHandler : IRequestHandler<GetAllVacanciesQuery, IEnumerable<VacancySummaryDto>>
{
    private readonly IVacancyRepository _repository;

    public GetAllVacanciesQueryHandler(IVacancyRepository repository) => _repository = repository;

    public async Task<IEnumerable<VacancySummaryDto>> Handle(GetAllVacanciesQuery query, CancellationToken ct)
    {
        var vacancies = query.OpenOnly
            ? await _repository.GetAllOpenAsync(ct)
            : (await _repository.GetAllOpenAsync(ct)); // extend with GetAll when needed

        return vacancies.Select(v => new VacancySummaryDto(
            v.VacancyId, v.VacancyName, v.Designation, v.VacancyUnit,
            v.VacancyLocation, v.VacancyProcess, v.VacancyLastDate,
            v.LiveStatus.ToCode(), v.NumberOfOpenings, v.CtcFrom, v.CtcTo));
    }
}

// ── Get By ID ─────────────────────────────────────────────────────────────────

public record GetVacancyByIdQuery(decimal VacancyId) : IRequest<VacancyDto>;

public class GetVacancyByIdQueryHandler : IRequestHandler<GetVacancyByIdQuery, VacancyDto>
{
    private readonly IVacancyRepository _repository;

    public GetVacancyByIdQueryHandler(IVacancyRepository repository) => _repository = repository;

    public async Task<VacancyDto> Handle(GetVacancyByIdQuery query, CancellationToken ct)
    {
        var v = await _repository.GetByIdAsync(query.VacancyId, ct)
            ?? throw new VacancyNotFoundException(query.VacancyId);

        return new VacancyDto(
            v.VacancyId, v.VacancyUnit, v.VacancyGrade, v.VacancyPositionId,
            v.VacancyName, v.VacancyReporting, v.VacancyLocation, v.VacancyProcess,
            v.VacancyAge, v.VacancyExperience, v.VacancyQualification,
            v.VacancyNarration1, v.VacancyNarration2, v.VacancyNarration3, v.VacancyNarration4,
            v.VacancyAttachment, v.VacancyLastDate, v.AdvertiseIntranet, v.AdvertiseInternet,
            v.LiveStatus.ToCode(), v.NumberOfOpenings, v.CtcFrom, v.CtcTo,
            v.Designation, v.VacancyType, v.InternalReferralAllowed, v.InternalReferralEmail,
            v.PostedDate, v.Remarks, v.DisabilityFlag);
    }
}

// ── Get By Unit ───────────────────────────────────────────────────────────────

public record GetVacanciesByUnitQuery(string Unit) : IRequest<IEnumerable<VacancySummaryDto>>;

public class GetVacanciesByUnitQueryHandler : IRequestHandler<GetVacanciesByUnitQuery, IEnumerable<VacancySummaryDto>>
{
    private readonly IVacancyRepository _repository;

    public GetVacanciesByUnitQueryHandler(IVacancyRepository repository) => _repository = repository;

    public async Task<IEnumerable<VacancySummaryDto>> Handle(GetVacanciesByUnitQuery query, CancellationToken ct)
    {
        var vacancies = await _repository.GetByUnitAsync(query.Unit, ct);
        return vacancies.Select(v => new VacancySummaryDto(
            v.VacancyId, v.VacancyName, v.Designation, v.VacancyUnit,
            v.VacancyLocation, v.VacancyProcess, v.VacancyLastDate,
            v.LiveStatus.ToCode(), v.NumberOfOpenings, v.CtcFrom, v.CtcTo));
    }
}
