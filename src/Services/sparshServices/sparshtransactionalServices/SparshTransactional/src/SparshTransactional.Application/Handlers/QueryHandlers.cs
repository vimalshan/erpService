using AutoMapper;
using MediatR;
using SparshTransactional.Application.DTOs;
using SparshTransactional.Application.Queries;
using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Application.Handlers;

public class GetScholarshipByIdHandler(
    IScholarshipMasterRepository repo,
    IMapper mapper) : IRequestHandler<GetScholarshipByIdQuery, ScholarshipMasterDto?>
{
    public async Task<ScholarshipMasterDto?> Handle(GetScholarshipByIdQuery request, CancellationToken ct)
    {
        var scholarship = await repo.GetByIdAsync(request.ScholarshipId, ct);
        return scholarship is null ? null : mapper.Map<ScholarshipMasterDto>(scholarship);
    }
}

public class GetAllScholarshipsHandler(
    IScholarshipMasterRepository repo,
    IMapper mapper) : IRequestHandler<GetAllScholarshipsQuery, IReadOnlyList<ScholarshipMasterDto>>
{
    public async Task<IReadOnlyList<ScholarshipMasterDto>> Handle(GetAllScholarshipsQuery request, CancellationToken ct)
    {
        var scholarships = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ScholarshipMasterDto>>(scholarships);
    }
}

public class GetActiveScholarshipsHandler(
    IScholarshipMasterRepository repo,
    IMapper mapper) : IRequestHandler<GetActiveScholarshipsQuery, IReadOnlyList<ScholarshipMasterDto>>
{
    public async Task<IReadOnlyList<ScholarshipMasterDto>> Handle(GetActiveScholarshipsQuery request, CancellationToken ct)
    {
        var scholarships = await repo.GetActiveAsync(ct);
        return mapper.Map<IReadOnlyList<ScholarshipMasterDto>>(scholarships);
    }
}

public class GetEligibilityCriteriaByScholarshipHandler(
    IEligibilityCriteriaRepository repo,
    IMapper mapper) : IRequestHandler<GetEligibilityCriteriaByScholarshipQuery, IReadOnlyList<EligibilityCriteriaDto>>
{
    public async Task<IReadOnlyList<EligibilityCriteriaDto>> Handle(GetEligibilityCriteriaByScholarshipQuery request, CancellationToken ct)
    {
        var criteria = await repo.GetByScholarshipIdAsync(request.ScholarshipId, ct);
        return mapper.Map<IReadOnlyList<EligibilityCriteriaDto>>(criteria);
    }
}

public class GetApplicationByIdHandler(
    IScholarshipApplicationRepository repo,
    IMapper mapper) : IRequestHandler<GetApplicationByIdQuery, ScholarshipApplicationDto?>
{
    public async Task<ScholarshipApplicationDto?> Handle(GetApplicationByIdQuery request, CancellationToken ct)
    {
        var application = await repo.GetByIdAsync(request.ApplicationId, ct);
        return application is null ? null : mapper.Map<ScholarshipApplicationDto>(application);
    }
}

public class GetAllApplicationsHandler(
    IScholarshipApplicationRepository repo,
    IMapper mapper) : IRequestHandler<GetAllApplicationsQuery, IReadOnlyList<ScholarshipApplicationDto>>
{
    public async Task<IReadOnlyList<ScholarshipApplicationDto>> Handle(GetAllApplicationsQuery request, CancellationToken ct)
    {
        var applications = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ScholarshipApplicationDto>>(applications);
    }
}

public class GetApplicationsByStatusHandler(
    IScholarshipApplicationRepository repo,
    IMapper mapper) : IRequestHandler<GetApplicationsByStatusQuery, IReadOnlyList<ScholarshipApplicationDto>>
{
    public async Task<IReadOnlyList<ScholarshipApplicationDto>> Handle(GetApplicationsByStatusQuery request, CancellationToken ct)
    {
        var applications = await repo.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<ScholarshipApplicationDto>>(applications);
    }
}

public class GetApplicationsByStudentHandler(
    IScholarshipApplicationRepository repo,
    IMapper mapper) : IRequestHandler<GetApplicationsByStudentQuery, IReadOnlyList<ScholarshipApplicationDto>>
{
    public async Task<IReadOnlyList<ScholarshipApplicationDto>> Handle(GetApplicationsByStudentQuery request, CancellationToken ct)
    {
        var applications = await repo.GetByStudentIdAsync(request.StudentId, ct);
        return mapper.Map<IReadOnlyList<ScholarshipApplicationDto>>(applications);
    }
}

public class GetApplicationsByScholarshipHandler(
    IScholarshipApplicationRepository repo,
    IMapper mapper) : IRequestHandler<GetApplicationsByScholarshipQuery, IReadOnlyList<ScholarshipApplicationDto>>
{
    public async Task<IReadOnlyList<ScholarshipApplicationDto>> Handle(GetApplicationsByScholarshipQuery request, CancellationToken ct)
    {
        var applications = await repo.GetByScholarshipIdAsync(request.ScholarshipId, ct);
        return mapper.Map<IReadOnlyList<ScholarshipApplicationDto>>(applications);
    }
}

public class GetDisbursementByIdHandler(
    IScholarshipDisbursementRepository repo,
    IMapper mapper) : IRequestHandler<GetDisbursementByIdQuery, ScholarshipDisbursementDto?>
{
    public async Task<ScholarshipDisbursementDto?> Handle(GetDisbursementByIdQuery request, CancellationToken ct)
    {
        var disbursement = await repo.GetByIdAsync(request.DisbursementId, ct);
        return disbursement is null ? null : mapper.Map<ScholarshipDisbursementDto>(disbursement);
    }
}

public class GetDisbursementsByApplicationHandler(
    IScholarshipDisbursementRepository repo,
    IMapper mapper) : IRequestHandler<GetDisbursementsByApplicationQuery, IReadOnlyList<ScholarshipDisbursementDto>>
{
    public async Task<IReadOnlyList<ScholarshipDisbursementDto>> Handle(GetDisbursementsByApplicationQuery request, CancellationToken ct)
    {
        var disbursements = await repo.GetByApplicationIdAsync(request.ApplicationId, ct);
        return mapper.Map<IReadOnlyList<ScholarshipDisbursementDto>>(disbursements);
    }
}

public class GetDisbursementsByStatusHandler(
    IScholarshipDisbursementRepository repo,
    IMapper mapper) : IRequestHandler<GetDisbursementsByStatusQuery, IReadOnlyList<ScholarshipDisbursementDto>>
{
    public async Task<IReadOnlyList<ScholarshipDisbursementDto>> Handle(GetDisbursementsByStatusQuery request, CancellationToken ct)
    {
        var disbursements = await repo.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<ScholarshipDisbursementDto>>(disbursements);
    }
}

public class GetAllDisbursementsHandler(
    IScholarshipDisbursementRepository repo,
    IMapper mapper) : IRequestHandler<GetAllDisbursementsQuery, IReadOnlyList<ScholarshipDisbursementDto>>
{
    public async Task<IReadOnlyList<ScholarshipDisbursementDto>> Handle(GetAllDisbursementsQuery request, CancellationToken ct)
    {
        var disbursements = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ScholarshipDisbursementDto>>(disbursements);
    }
}
