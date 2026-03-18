using AutoMapper;
using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Interfaces;

namespace WorkOrderService.Application.Queries.GetAllWorkOrders;

public class GetAllWorkOrdersQueryHandler : IRequestHandler<GetAllWorkOrdersQuery, IEnumerable<WorkOrderDto>>
{
    private readonly IWorkOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetAllWorkOrdersQueryHandler(IWorkOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WorkOrderDto>> Handle(GetAllWorkOrdersQuery request, CancellationToken cancellationToken)
    {
        var workOrders = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
    }
}
