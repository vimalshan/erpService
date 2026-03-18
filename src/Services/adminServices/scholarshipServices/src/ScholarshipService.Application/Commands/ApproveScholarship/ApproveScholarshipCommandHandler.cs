using MediatR;
using Microsoft.Extensions.Logging;
using ScholarshipService.Application.Common;
using ScholarshipService.Domain.Repositories;

namespace ScholarshipService.Application.Commands.ApproveScholarship;

public class ApproveScholarshipCommandHandler(
    IScholarshipMainRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<ApproveScholarshipCommandHandler> logger)
    : IRequestHandler<ApproveScholarshipCommand, bool>
{
    public async Task<bool> Handle(ApproveScholarshipCommand request, CancellationToken cancellationToken)
    {
        var scholarship = await repository.GetByIdAsync(request.ScholarshipId, cancellationToken)
            ?? throw new KeyNotFoundException($"Scholarship {request.ScholarshipId} not found.");

        scholarship.Approve(request.ApprovedBy, request.Remarks);
        await repository.UpdateAsync(scholarship, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Scholarship {ScholarshipId} approved by {ApprovedBy}", request.ScholarshipId, request.ApprovedBy);
        return true;
    }
}
