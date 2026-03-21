using AutoMapper;
using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Application.Queries;

public class GetTravelRequestsByUserQueryHandler : IRequestHandler<GetTravelRequestsByUserQuery, IReadOnlyList<TravelRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTravelRequestsByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TravelRequestDto>> Handle(GetTravelRequestsByUserQuery request, CancellationToken cancellationToken)
    {
        var travelRequests = await _unitOfWork.TravelRequests.GetByUserAsync(request.UserNumber, cancellationToken);
        return _mapper.Map<IReadOnlyList<TravelRequestDto>>(travelRequests);
    }
}
