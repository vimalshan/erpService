using MediatR;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Entities;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Commands.SelfAssessment;

public record CreateSelfAssessmentCommand : IRequest<long>
{
    public char AssessmentType { get; init; }
    public long TypeReferenceId { get; init; }
    public string MonitoredBy { get; init; } = default!;
    public DateTime DueDate { get; init; }
    public long CreatedBy { get; init; }
}

public class CreateSelfAssessmentCommandHandler(
    ISelfAssessmentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSelfAssessmentCommand, long>
{
    public async Task<long> Handle(CreateSelfAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = new RiskSelfAssessment
        {
            AssessmentType = request.AssessmentType,
            TypeReferenceId = request.TypeReferenceId,
            MonitoredBy = request.MonitoredBy,
            DueDate = request.DueDate,
            MeetingFlag = 'P',
            Status = 'E',
            AssessmentDate = DateTime.UtcNow,
            ReviewFlag = 'N',
            NewRiskFlag = 'N',
            MitigationFlag = 'N',
            ApprovalStatus = 'P',
            LastModifiedBy = request.CreatedBy,
            LastModifiedOn = DateTime.UtcNow,
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        await repository.AddAsync(assessment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return assessment.Id;
    }
}

public record CompleteSelfAssessmentCommand(long AssessmentId, long CompletedBy) : IRequest<bool>;

public class CompleteSelfAssessmentCommandHandler(
    ISelfAssessmentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CompleteSelfAssessmentCommand, bool>
{
    public async Task<bool> Handle(CompleteSelfAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = await repository.GetByIdAsync(request.AssessmentId, cancellationToken);
        if (assessment is null) return false;

        assessment.Complete(request.CompletedBy);
        repository.Update(assessment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
