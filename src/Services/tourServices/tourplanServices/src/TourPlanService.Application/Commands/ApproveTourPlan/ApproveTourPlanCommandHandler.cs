using MediatR;
using TourPlanService.Application.Common;
using TourPlanService.Application.Interfaces;
using TourPlanService.Domain.Exceptions;
using TourPlanService.Domain.Interfaces;

namespace TourPlanService.Application.Commands.ApproveTourPlan;

public sealed class ApproveTourPlanCommandHandler(
    ITourPlanRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ApproveTourPlanCommand, Result>
{
    public async Task<Result> Handle(ApproveTourPlanCommand request, CancellationToken cancellationToken)
    {
        var tourPlan = await repository.GetByIdAsync(request.TpId, cancellationToken);
        if (tourPlan is null)
            return Result.Failure($"Tour plan '{request.TpId}' not found.");

        try
        {
            tourPlan.Approve(request.ApprovedBy, request.Remarks);
            repository.Update(tourPlan);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}

public sealed class RejectTourPlanCommandHandler(
    ITourPlanRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<RejectTourPlanCommand, Result>
{
    public async Task<Result> Handle(RejectTourPlanCommand request, CancellationToken cancellationToken)
    {
        var tourPlan = await repository.GetByIdAsync(request.TpId, cancellationToken);
        if (tourPlan is null)
            return Result.Failure($"Tour plan '{request.TpId}' not found.");

        tourPlan.Reject(request.RejectedBy, request.Remarks);
        repository.Update(tourPlan);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
