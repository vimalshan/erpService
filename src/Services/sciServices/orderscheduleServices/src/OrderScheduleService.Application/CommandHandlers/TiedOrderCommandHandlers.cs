namespace OrderScheduleService.Application.CommandHandlers;

using MediatR;
using AutoMapper;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Domain.Interfaces;
using OrderScheduleService.Domain.Aggregates;

public class CreateTiedOrderCommandHandler : IRequestHandler<CreateTiedOrderCommand, long>
{
    private readonly ITiedOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateTiedOrderCommandHandler(ITiedOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateTiedOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new TiedOrderAggregate(
            0, // ID will be generated
            request.Order.CustomerCode,
            request.Order.CompanyUnitId,
            DateTime.UtcNow,
            request.Order.ModifiedUserId);

        foreach (var detail in request.Order.Details)
        {
            order.AddDetail(detail.ItemId, detail.ItemName, detail.OrderQuantity, detail.DispatchDate, detail.Price);
        }

        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();

        return order.Id;
    }
}

public class AddOrderDetailCommandHandler : IRequestHandler<AddOrderDetailCommand, long>
{
    private readonly ITiedOrderRepository _repository;

    public AddOrderDetailCommandHandler(ITiedOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(AddOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        if (order == null)
            throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.AddDetail(request.ItemId, request.ItemName, request.OrderQuantity, request.DispatchDate, request.Price);

        await _repository.UpdateAsync(order);
        await _repository.SaveChangesAsync();

        return order.Details.Last().Id;
    }
}

public class ScheduleOrderDetailCommandHandler : IRequestHandler<ScheduleOrderDetailCommand, bool>
{
    private readonly ITiedOrderRepository _repository;

    public ScheduleOrderDetailCommandHandler(ITiedOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ScheduleOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        if (order == null)
            throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.ScheduleDetail(request.DetailId, request.ScheduledDate, request.AllocatedQuantity, request.UserId);

        await _repository.UpdateAsync(order);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class CancelOrderDetailCommandHandler : IRequestHandler<CancelOrderDetailCommand, bool>
{
    private readonly ITiedOrderRepository _repository;

    public CancelOrderDetailCommandHandler(ITiedOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CancelOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        if (order == null)
            throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.CancelDetail(request.DetailId, request.UserId);

        await _repository.UpdateAsync(order);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly ITiedOrderRepository _repository;

    public UpdateOrderStatusCommandHandler(ITiedOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        if (order == null)
            throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.UpdateStatus(request.Status, request.UserId);

        await _repository.UpdateAsync(order);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class DeleteTiedOrderCommandHandler : IRequestHandler<DeleteTiedOrderCommand, bool>
{
    private readonly ITiedOrderRepository _repository;

    public DeleteTiedOrderCommandHandler(ITiedOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTiedOrderCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.OrderId);
        await _repository.SaveChangesAsync();

        return true;
    }
}
