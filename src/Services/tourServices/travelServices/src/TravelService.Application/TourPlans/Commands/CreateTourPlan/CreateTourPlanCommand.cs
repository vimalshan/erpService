using MediatR;
using TravelService.Application.Common.Interfaces;
using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;
using TravelService.Domain.ValueObjects;

namespace TravelService.Application.TourPlans.Commands.CreateTourPlan;

public record CreateTourPlanCommand(
    string EmployeeSysId,
    DateTime StartDate,
    DateTime? EndDate,
    string Purpose,
    string Remarks,
    string Category,
    bool IncludeBookingRequests,
    string FromCityId,
    string FromCityName,
    string ToCityId,
    string ToCityName,
    string SupervisorRemarks,
    string? GradeType,
    string? ContactNo,
    string? TripType,
    string PayrollUnitId
) : IRequest<TourPlanDto>;

public class CreateTourPlanHandler : IRequestHandler<CreateTourPlanCommand, TourPlanDto>
{
    private readonly ITourPlanRepository _repository;
    private readonly IMessagePublisher _messagePublisher;

    public CreateTourPlanHandler(ITourPlanRepository repository, IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _messagePublisher = messagePublisher;
    }

    public async Task<TourPlanDto> Handle(CreateTourPlanCommand request, CancellationToken cancellationToken)
    {
        var id = $"TP{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        var fromCity = new CityInfo(request.FromCityId, request.FromCityName);
        var toCity = new CityInfo(request.ToCityId, request.ToCityName);
        var tourPlan = Domain.Entities.TourPlan.TourPlan.Create(
            id, request.EmployeeSysId, request.StartDate, request.EndDate,
            request.Purpose, request.Remarks, request.Category, request.IncludeBookingRequests,
            fromCity, toCity, request.SupervisorRemarks, request.EmployeeSysId,
            request.PayrollUnitId, request.TripType, request.GradeType, request.ContactNo);

        await _repository.AddAsync(tourPlan, cancellationToken);
        await _messagePublisher.PublishAsync("travel.events", "tourplan.created",
            new { tourPlan.Id, tourPlan.EmployeeSysId }, cancellationToken);

        return MapToDto(tourPlan);
    }

    private static TourPlanDto MapToDto(Domain.Entities.TourPlan.TourPlan tp) => new()
    {
        Id = tp.Id,
        EmployeeSysId = tp.EmployeeSysId,
        StartDate = tp.StartDate,
        EndDate = tp.EndDate,
        Purpose = tp.Purpose,
        Remarks = tp.Remarks,
        Status = tp.Status,
        Category = tp.Category,
        IncludeBookingRequests = tp.IncludeBookingRequests,
        TripType = tp.TripType,
        CreatedBy = tp.CreatedBy,
        CreatedOn = tp.CreatedOn,
        FromCityId = tp.FromCity.CityId,
        FromCityName = tp.FromCity.CityName,
        ToCityId = tp.ToCity.CityId,
        ToCityName = tp.ToCity.CityName,
        SupervisorRemarks = tp.SupervisorRemarks,
        ContactNo = tp.ContactNo,
        GradeType = tp.GradeType,
        PayrollUnitId = tp.PayrollUnitId
    };
}
