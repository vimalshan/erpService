using MediatR;
using Microsoft.Extensions.Logging;
using ScholarshipService.Application.Common;
using ScholarshipService.Domain.Repositories;

namespace ScholarshipService.Application.Commands.StopScholarship;

public class StopScholarshipCommandHandler(
    IScholarshipMainRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<StopScholarshipCommandHandler> logger)
    : IRequestHandler<StopScholarshipCommand, bool>
{
    public async Task<bool> Handle(StopScholarshipCommand request, CancellationToken cancellationToken)
    {
        var scholarship = await repository.GetByIdAsync(request.ScholarshipId, cancellationToken)
            ?? throw new KeyNotFoundException($"Scholarship {request.ScholarshipId} not found.");

        scholarship.Stop(request.Reason, request.StoppedBy);
        await repository.UpdateAsync(scholarship, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Scholarship {ScholarshipId} stopped by {StoppedBy}", request.ScholarshipId, request.StoppedBy);
        return true;
    }
}
