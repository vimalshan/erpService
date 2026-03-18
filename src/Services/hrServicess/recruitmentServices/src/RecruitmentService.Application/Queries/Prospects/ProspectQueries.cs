using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Domain.Exceptions;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Application.Queries.Prospects;

// ── Get By ID ─────────────────────────────────────────────────────────────────

public record GetProspectByIdQuery(decimal UserId) : IRequest<ProspectDto>;

public class GetProspectByIdQueryHandler : IRequestHandler<GetProspectByIdQuery, ProspectDto>
{
    private readonly IProspectRepository _repository;

    public GetProspectByIdQueryHandler(IProspectRepository repository) => _repository = repository;

    public async Task<ProspectDto> Handle(GetProspectByIdQuery query, CancellationToken ct)
    {
        var p = await _repository.GetByIdAsync(query.UserId, ct)
            ?? throw new ProspectNotFoundException(query.UserId);

        return MapToDto(p);
    }

    internal static ProspectDto MapToDto(Domain.Entities.Prospect p) => new(
        p.WebUserId, p.FirstName, p.MiddleName, p.LastName, p.EmailId,
        p.Status.ToCode(), p.DateOfBirth, p.CreatedOn, p.ProspectType,
        p.Addresses.Select(a => new ProspectAddressDto(
            a.EmpSysId, a.AddressFlag, a.Address1, a.Address2, a.Address3,
            a.Address4, a.City, a.PinCode, a.MobileNo, a.LandlineNo)),
        p.Qualifications.Select(q => new ProspectQualificationDto(
            q.EmpSysId, q.QualId, q.QualCode, q.QualDescription, q.YearFrom, q.YearTo,
            q.InstitutionCode, q.InstitutionDescription, q.EducationType,
            q.SpecializationCode, q.SpecializationDescription, q.Percentage,
            q.DegreeCode, q.DegreeDescription)),
        p.References.Select(r => new ProspectReferenceDto(
            r.EmpSysId, r.RefId, r.Name, r.Designation, r.Address1, r.Address2,
            r.Phone, r.Email)),
        p.Trainings.Select(t => new ProspectTrainingDto(
            t.EmpSysId, t.TrainingId, t.Title, t.Duration, t.Institute, t.Location)));
}

// ── Get All ───────────────────────────────────────────────────────────────────

public record GetAllProspectsQuery : IRequest<IEnumerable<ProspectSummaryDto>>;

public class GetAllProspectsQueryHandler : IRequestHandler<GetAllProspectsQuery, IEnumerable<ProspectSummaryDto>>
{
    private readonly IProspectRepository _repository;

    public GetAllProspectsQueryHandler(IProspectRepository repository) => _repository = repository;

    public async Task<IEnumerable<ProspectSummaryDto>> Handle(GetAllProspectsQuery query, CancellationToken ct)
    {
        var prospects = await _repository.GetAllAsync(ct);
        return prospects.Select(p => new ProspectSummaryDto(
            p.WebUserId, p.FullName, p.EmailId, p.Status.ToCode(), p.DateOfBirth, p.CreatedOn));
    }
}
