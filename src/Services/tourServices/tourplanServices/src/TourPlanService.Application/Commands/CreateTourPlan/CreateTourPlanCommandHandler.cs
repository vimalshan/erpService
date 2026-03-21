using MediatR;
using TourPlanService.Application.Common;
using TourPlanService.Application.Interfaces;
using TourPlanService.Domain.Entities;
using TourPlanService.Domain.Interfaces;

namespace TourPlanService.Application.Commands.CreateTourPlan;

public sealed class CreateTourPlanCommandHandler(
    ITourPlanRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTourPlanCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateTourPlanCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsAsync(request.TpId, cancellationToken))
            return Result<string>.Failure($"Tour plan with ID '{request.TpId}' already exists.");

        var tourPlan = TourPlan.Create(
            request.TpId,
            request.TpEmpSysId,
            request.TpStartDate,
            request.TpPurpose,
            request.TpRemarks,
            request.TpCategory,
            request.TpBookInc,
            request.TpFromCityId,
            request.TpFromCityName,
            request.TpToCityId,
            request.TpToCityName,
            request.TpSupRemarks,
            request.CreatedBy);

        await repository.AddAsync(tourPlan, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(tourPlan.TpId);
    }
}
