using MediatR;

namespace CompensationService.Application.Commands;

/// <summary>
/// Command to change compensation grade status
/// </summary>
public class ChangeCompensationGradeStatusCommand : IRequest<bool>
{
    public long GradeId { get; set; }
    public char NewStatus { get; set; }
    public long ChangedBy { get; set; }
}

/// <summary>
/// Handler for ChangeCompensationGradeStatusCommand
/// </summary>
public class ChangeCompensationGradeStatusCommandHandler : IRequestHandler<ChangeCompensationGradeStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeCompensationGradeStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeCompensationGradeStatusCommand request, CancellationToken cancellationToken)
    {
        var grade = await _unitOfWork.CompensationGrades.GetByIdAsync(request.GradeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Compensation grade with ID {request.GradeId} not found.");

        grade.ChangeStatus(request.NewStatus, request.ChangedBy);
        await _unitOfWork.CompensationGrades.UpdateAsync(grade, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
