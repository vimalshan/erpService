using MediatR;
using TourPlanService.Application.Common;
using TourPlanService.Application.Interfaces;
using TourPlanService.Domain.Entities;
using TourPlanService.Domain.Interfaces;

namespace TourPlanService.Application.Commands.CreateForexRequisition;

public sealed class CreateForexRequisitionCommandHandler(
    IForexRepository forexRepository,
    ITourPlanRepository tourPlanRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateForexRequisitionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateForexRequisitionCommand request, CancellationToken cancellationToken)
    {
        var tourPlan = await tourPlanRepository.GetByIdAsync(request.TpId, cancellationToken);
        if (tourPlan is null)
            return Result<string>.Failure($"Tour plan '{request.TpId}' not found.");

        var forex = ForexRequisition.Create(
            request.ForReqId,
            request.TpId,
            request.PassNo,
            request.PassName,
            request.PassLocation,
            request.PassExpDate,
            request.Type,
            request.AdlRemarks,
            request.AdvRefNo,
            request.CreatedBy);

        await forexRepository.AddAsync(forex, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(forex.ForReqId);
    }
}
