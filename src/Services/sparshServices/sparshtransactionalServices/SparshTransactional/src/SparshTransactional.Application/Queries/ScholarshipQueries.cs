using MediatR;
using SparshTransactional.Application.DTOs;

namespace SparshTransactional.Application.Queries;

public record GetScholarshipByIdQuery(long ScholarshipId) : IRequest<ScholarshipMasterDto?>;

public record GetAllScholarshipsQuery : IRequest<IReadOnlyList<ScholarshipMasterDto>>;

public record GetActiveScholarshipsQuery : IRequest<IReadOnlyList<ScholarshipMasterDto>>;

public record GetEligibilityCriteriaByScholarshipQuery(long ScholarshipId) : IRequest<IReadOnlyList<EligibilityCriteriaDto>>;

public record GetApplicationByIdQuery(long ApplicationId) : IRequest<ScholarshipApplicationDto?>;

public record GetAllApplicationsQuery : IRequest<IReadOnlyList<ScholarshipApplicationDto>>;

public record GetApplicationsByStatusQuery(string Status) : IRequest<IReadOnlyList<ScholarshipApplicationDto>>;

public record GetApplicationsByStudentQuery(long StudentId) : IRequest<IReadOnlyList<ScholarshipApplicationDto>>;

public record GetApplicationsByScholarshipQuery(long ScholarshipId) : IRequest<IReadOnlyList<ScholarshipApplicationDto>>;

public record GetDisbursementByIdQuery(long DisbursementId) : IRequest<ScholarshipDisbursementDto?>;

public record GetDisbursementsByApplicationQuery(long ApplicationId) : IRequest<IReadOnlyList<ScholarshipDisbursementDto>>;

public record GetDisbursementsByStatusQuery(string Status) : IRequest<IReadOnlyList<ScholarshipDisbursementDto>>;

public record GetAllDisbursementsQuery : IRequest<IReadOnlyList<ScholarshipDisbursementDto>>;
