using MediatR;
using TravelService.Application.Common.Exceptions;
using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;

namespace TravelService.Application.TourPlans.Commands.RejectTourPlan;

public record RejectTourPlanCommand(string TourPlanId, string RejectedBy, string Remarks) : IRequest<TourPlanDto>;

public class RejectTourPlanHandler : IRequestHandler<RejectTourPlanCommand, TourPlanDto>
{
    private readonly ITourPlanRepository _repository;

    public RejectTourPlanHandler(ITourPlanRepository repository) => _repository = repository;

    public async Task<TourPlanDto> Handle(RejectTourPlanCommand request, CancellationToken cancellationToken)
    {
        var tourPlan = await _repository.GetByIdAsync(request.TourPlanId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TourPlan.TourPlan), request.TourPlanId);

        tourPlan.Reject(request.RejectedBy, request.Remarks);
        await _repository.UpdateAsync(tourPlan, cancellationToken);

        return new TourPlanDto { Id = tourPlan.Id, Status = tourPlan.Status, ApproverRemarks = tourPlan.ApproverRemarks };
    }
}
