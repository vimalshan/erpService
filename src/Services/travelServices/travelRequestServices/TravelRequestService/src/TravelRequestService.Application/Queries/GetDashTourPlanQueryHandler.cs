using AutoMapper;
using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Application.Interfaces;

namespace TravelRequestService.Application.Queries;

public class GetDashTourPlanQueryHandler : IRequestHandler<GetDashTourPlanQuery, IReadOnlyList<DashTourPlanDto>>
{
    private readonly IDapperQueryService _dapperQueryService;
    private readonly IMapper _mapper;

    public GetDashTourPlanQueryHandler(IDapperQueryService dapperQueryService, IMapper mapper)
    {
        _dapperQueryService = dapperQueryService;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DashTourPlanDto>> Handle(GetDashTourPlanQuery request, CancellationToken cancellationToken)
    {
        return await _dapperQueryService.GetDashTourPlansAsync(cancellationToken);
    }
}
