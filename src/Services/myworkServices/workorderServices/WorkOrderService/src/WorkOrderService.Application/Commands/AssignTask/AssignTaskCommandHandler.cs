using AutoMapper;
using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Entities;
using WorkOrderService.Domain.Interfaces;

namespace WorkOrderService.Application.Commands.AssignTask;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, WorkTaskDto>
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IWorkTaskRepository _workTaskRepository;
    private readonly IMapper _mapper;

    public AssignTaskCommandHandler(
        IWorkOrderRepository workOrderRepository,
        IWorkTaskRepository workTaskRepository,
        IMapper mapper)
    {
        _workOrderRepository = workOrderRepository;
        _workTaskRepository = workTaskRepository;
        _mapper = mapper;
    }

    public async Task<WorkTaskDto> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(request.WorkOrderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Work order {request.WorkOrderId} not found.");

        var task = new WorkTask(
            request.WorkOrderId,
            request.TaskName,
            request.AssignedTo,
            request.EstimatedHours,
            request.CreatedBy);

        var created = await _workTaskRepository.AddAsync(task, cancellationToken);
        return _mapper.Map<WorkTaskDto>(created);
    }
}
