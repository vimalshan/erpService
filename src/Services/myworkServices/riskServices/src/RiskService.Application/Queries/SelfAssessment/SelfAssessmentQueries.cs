using MediatR;
using RiskService.Application.DTOs;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Queries.SelfAssessment;

public record GetPendingSelfAssessmentsQuery : IRequest<IReadOnlyList<SelfAssessmentDto>>;

public class GetPendingSelfAssessmentsQueryHandler(ISelfAssessmentRepository repository)
    : IRequestHandler<GetPendingSelfAssessmentsQuery, IReadOnlyList<SelfAssessmentDto>>
{
    public async Task<IReadOnlyList<SelfAssessmentDto>> Handle(GetPendingSelfAssessmentsQuery request, CancellationToken cancellationToken)
    {
        var assessments = await repository.GetPendingAsync(cancellationToken);
        return assessments.Select(a => new SelfAssessmentDto
        {
            Id = a.Id,
            AssessmentType = a.AssessmentType.ToString(),
            TypeReferenceId = a.TypeReferenceId,
            MonitoredBy = a.MonitoredBy,
            DueDate = a.DueDate,
            MeetingFlag = a.MeetingFlag.ToString(),
            Status = a.Status.ToString(),
            Reason = a.Reason,
            AssessmentDate = a.AssessmentDate,
            ApprovalStatus = a.ApprovalStatus.ToString()
        }).ToList();
    }
}
