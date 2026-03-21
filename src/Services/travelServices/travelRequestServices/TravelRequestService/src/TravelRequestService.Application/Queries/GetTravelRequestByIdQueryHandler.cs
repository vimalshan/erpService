using AutoMapper;
using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Application.Queries;

public class GetTravelRequestByIdQueryHandler : IRequestHandler<GetTravelRequestByIdQuery, TravelRequestDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTravelRequestByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TravelRequestDto?> Handle(GetTravelRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var travelRequest = await _unitOfWork.TravelRequests.GetByIdAsync(
            request.PlanNumber, request.CompanyCode, cancellationToken);

        return travelRequest is null ? null : _mapper.Map<TravelRequestDto>(travelRequest);
    }
}
