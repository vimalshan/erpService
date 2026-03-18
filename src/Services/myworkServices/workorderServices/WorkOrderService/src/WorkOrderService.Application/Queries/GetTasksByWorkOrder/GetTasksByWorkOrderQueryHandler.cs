using AutoMapper;
using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Interfaces;

namespace WorkOrderService.Application.Queries.GetTasksByWorkOrder;

public class GetTasksByWorkOrderQueryHandler : IRequestHandler<GetTasksByWorkOrderQuery, IEnumerable<WorkTaskDto>>
{
    private readonly IWorkTaskRepository _repository;
    private readonly IMapper _mapper;

    public GetTasksByWorkOrderQueryHandler(IWorkTaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WorkTaskDto>> Handle(GetTasksByWorkOrderQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetByWorkOrderIdAsync(request.WorkOrderId, cancellationToken);
        return _mapper.Map<IEnumerable<WorkTaskDto>>(tasks);
    }
}
