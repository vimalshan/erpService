using AutoMapper;
using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Application.Queries;

public class GetAllTravelRequestsQueryHandler : IRequestHandler<GetAllTravelRequestsQuery, IReadOnlyList<TravelRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTravelRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TravelRequestDto>> Handle(GetAllTravelRequestsQuery request, CancellationToken cancellationToken)
    {
        var travelRequests = await _unitOfWork.TravelRequests.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<TravelRequestDto>>(travelRequests);
    }
}
