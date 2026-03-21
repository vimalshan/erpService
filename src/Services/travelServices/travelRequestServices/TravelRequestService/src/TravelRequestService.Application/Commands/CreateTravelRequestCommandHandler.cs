using AutoMapper;
using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Enums;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Application.Commands;

public class CreateTravelRequestCommandHandler : IRequestHandler<CreateTravelRequestCommand, TravelRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTravelRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TravelRequestDto> Handle(CreateTravelRequestCommand request, CancellationToken cancellationToken)
    {
        var existingRequests = await _unitOfWork.TravelRequests.GetAllAsync(cancellationToken);
        var nextPlanNumber = existingRequests.Count > 0
            ? existingRequests.Max(x => x.PlanNumber) + 1
            : 1;

        var travelType = Enum.Parse<TravelType>(request.TravelType, ignoreCase: true);

        var travelRequest = TravelMain.Create(
            request.CompanyCode,
            nextPlanNumber,
            request.UserNumber,
            request.Objective,
            travelType,
            request.BudgetAmount);

        for (int i = 0; i < request.Agendas.Count; i++)
        {
            var agendaItem = request.Agendas[i];
            var agenda = TravelAgenda.Create(
                nextPlanNumber,
                i + 1,
                agendaItem.MeetingDate,
                agendaItem.PeopleToMeet,
                agendaItem.DesiredOutcome,
                agendaItem.CityName);
            travelRequest.AddAgenda(agenda);
        }

        await _unitOfWork.TravelRequests.AddAsync(travelRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TravelRequestDto>(travelRequest);
    }
}
