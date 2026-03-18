using AutoMapper;
using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Interfaces;

namespace WorkOrderService.Application.Commands.CompleteTask;

public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, WorkTaskDto>
{
    private readonly IWorkTaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public CompleteTaskCommandHandler(IWorkTaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<WorkTaskDto> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken)
            ?? throw new KeyNotFoundException($"Task {request.TaskId} not found.");

        task.Complete(request.ActualHours, request.CompletionRemarks, request.CompletedBy);
        await _taskRepository.UpdateAsync(task, cancellationToken);

        return _mapper.Map<WorkTaskDto>(task);
    }
}
