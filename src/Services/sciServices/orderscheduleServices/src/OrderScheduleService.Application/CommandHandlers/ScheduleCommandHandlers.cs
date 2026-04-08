namespace OrderScheduleService.Application.CommandHandlers;

using MediatR;
using AutoMapper;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Domain.Interfaces;
using OrderScheduleService.Domain.Aggregates;

public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, long>
{
    private readonly IScheduleRepository _repository;
    private readonly IMapper _mapper;

    public CreateScheduleCommandHandler(IScheduleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = new ScheduleAggregate(
            0, // ID will be generated
            request.Schedule.FillingPointGroupId,
            request.Schedule.ItemId,
            request.Schedule.OrderType,
            request.Schedule.OrderId,
            request.Schedule.OrderLineId,
            request.Schedule.RequiredDate,
            request.Schedule.OrderQuantity,
            request.Schedule.ShiftCapacity);

        foreach (var detail in request.Schedule.Details)
        {
            schedule.AddScheduleDetail(
                detail.FillingDate,
                detail.FillingShift[0],
                detail.StartTime,
                detail.EndTime,
                detail.FillQuantity,
                detail.FillingPointGroupId);
        }

        await _repository.AddAsync(schedule);
        await _repository.SaveChangesAsync();

        return schedule.Id;
    }
}

public class AddScheduleDetailCommandHandler : IRequestHandler<AddScheduleDetailCommand, bool>
{
    private readonly IScheduleRepository _repository;

    public AddScheduleDetailCommandHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(AddScheduleDetailCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);
        if (schedule == null)
            throw new InvalidOperationException($"Schedule {request.ScheduleId} not found");

        schedule.AddScheduleDetail(
            request.FillingDate,
            request.FillingShift,
            request.StartTime,
            request.EndTime,
            request.FillQuantity,
            request.FillingPointGroupId);

        await _repository.UpdateAsync(schedule);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class ConfirmScheduleCommandHandler : IRequestHandler<ConfirmScheduleCommand, bool>
{
    private readonly IScheduleRepository _repository;

    public ConfirmScheduleCommandHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ConfirmScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);
        if (schedule == null)
            throw new InvalidOperationException($"Schedule {request.ScheduleId} not found");

        schedule.ConfirmSchedule();

        await _repository.UpdateAsync(schedule);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, bool>
{
    private readonly IScheduleRepository _repository;

    public DeleteScheduleCommandHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.ScheduleId);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class AllocateCapacityCommandHandler : IRequestHandler<AllocateCapacityCommand, bool>
{
    private readonly IScheduleRepository _repository;

    public AllocateCapacityCommandHandler(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(AllocateCapacityCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);
        if (schedule == null)
            throw new InvalidOperationException($"Schedule {request.ScheduleId} not found");

        if (!schedule.CanAllocateQuantity(request.Quantity))
            throw new InvalidOperationException("Insufficient capacity for allocation");

        await _repository.UpdateAsync(schedule);
        await _repository.SaveChangesAsync();

        return true;
    }
}
