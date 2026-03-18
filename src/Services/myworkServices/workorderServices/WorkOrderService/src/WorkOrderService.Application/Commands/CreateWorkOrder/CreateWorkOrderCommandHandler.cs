using AutoMapper;
using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Entities;
using WorkOrderService.Domain.Interfaces;

namespace WorkOrderService.Application.Commands.CreateWorkOrder;

public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, WorkOrderDto>
{
    private readonly IWorkOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateWorkOrderCommandHandler(IWorkOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WorkOrderDto> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var workOrder = new WorkOrder(
            request.WorkOrderName,
            request.Description,
            request.DueDate,
            request.AssignedTo,
            request.CreatedBy);

        var created = await _repository.AddAsync(workOrder, cancellationToken);
        return _mapper.Map<WorkOrderDto>(created);
    }
}
