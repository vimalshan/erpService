using MediatR;
using TravelService.Application.Common.Exceptions;
using TravelService.Application.Common.Interfaces;
using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;

namespace TravelService.Application.TourPlans.Commands.ApproveTourPlan;

public record ApproveTourPlanCommand(string TourPlanId, string ApprovedBy, string? Remarks) : IRequest<TourPlanDto>;

public class ApproveTourPlanHandler : IRequestHandler<ApproveTourPlanCommand, TourPlanDto>
{
    private readonly ITourPlanRepository _repository;
    private readonly IMessagePublisher _messagePublisher;

    public ApproveTourPlanHandler(ITourPlanRepository repository, IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _messagePublisher = messagePublisher;
    }

    public async Task<TourPlanDto> Handle(ApproveTourPlanCommand request, CancellationToken cancellationToken)
    {
        var tourPlan = await _repository.GetByIdAsync(request.TourPlanId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TourPlan.TourPlan), request.TourPlanId);

        tourPlan.Approve(request.ApprovedBy, request.Remarks);
        await _repository.UpdateAsync(tourPlan, cancellationToken);
        await _messagePublisher.PublishAsync("travel.events", "tourplan.approved",
            new { request.TourPlanId, request.ApprovedBy }, cancellationToken);

        return new TourPlanDto
        {
            Id = tourPlan.Id,
            EmployeeSysId = tourPlan.EmployeeSysId,
            Status = tourPlan.Status,
            ApprovedBy = tourPlan.ApprovedBy,
            ApprovedOn = tourPlan.ApprovedOn
        };
    }
}
