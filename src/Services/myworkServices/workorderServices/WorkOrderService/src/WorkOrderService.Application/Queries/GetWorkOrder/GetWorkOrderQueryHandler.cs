using AutoMapper;
using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Interfaces;

namespace WorkOrderService.Application.Queries.GetWorkOrder;

public class GetWorkOrderQueryHandler : IRequestHandler<GetWorkOrderQuery, WorkOrderDto?>
{
    private readonly IWorkOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetWorkOrderQueryHandler(IWorkOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkOrderDto?> Handle(GetWorkOrderQuery request, CancellationToken cancellationToken)
    {
        var workOrder = await _repository.GetByIdWithTasksAsync(request.WorkOrderId, cancellationToken);
        return workOrder is null ? null : _mapper.Map<WorkOrderDto>(workOrder);
    }
}
