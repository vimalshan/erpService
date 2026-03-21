using MediatR;
using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;

namespace TravelService.Application.TourPlans.Queries.GetTourPlan;

public record GetTourPlanByIdQuery(string Id) : IRequest<TourPlanDto?>;

public class GetTourPlanByIdHandler : IRequestHandler<GetTourPlanByIdQuery, TourPlanDto?>
{
    private readonly ITourPlanRepository _repository;

    public GetTourPlanByIdHandler(ITourPlanRepository repository) => _repository = repository;

    public async Task<TourPlanDto?> Handle(GetTourPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var tp = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tp is null) return null;

        return new TourPlanDto
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
            ApprovedBy = tp.ApprovedBy,
            ApprovedOn = tp.ApprovedOn,
            FromCityId = tp.FromCity.CityId,
            FromCityName = tp.FromCity.CityName,
            ToCityId = tp.ToCity.CityId,
            ToCityName = tp.ToCity.CityName,
            SupervisorRemarks = tp.SupervisorRemarks,
            ContactNo = tp.ContactNo,
            GradeType = tp.GradeType,
            PayrollUnitId = tp.PayrollUnitId,
            ClaimType = tp.ClaimType,
            ApproverRemarks = tp.ApproverRemarks,
            ExpenseStatus = tp.ExpenseStatus,
            ClosureStatus = tp.ClosureStatus,
            Advances = tp.Advances.Select(a => new TourPlanAdvanceDto
            {
                Id = a.Id,
                Amount = a.Amount,
                Currency = a.Currency,
                ApprovalStatus = a.ApprovalStatus,
                Remarks = a.Remarks
            }).ToList(),
            Agendas = tp.Agendas.Select(a => new TourPlanAgendaDto
            {
                Id = a.Id,
                City = a.City,
                PartyToMeet = a.PartyToMeet,
                DesiredOutcome = a.DesiredOutcome,
                AgendaDate = a.AgendaDate
            }).ToList()
        };
    }
}
