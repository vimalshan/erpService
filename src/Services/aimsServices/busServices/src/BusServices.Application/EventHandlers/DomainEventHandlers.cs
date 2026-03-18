using BusServices.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BusServices.Application.EventHandlers;

public sealed class BusRegisteredEventHandler : INotificationHandler<BusRegisteredEvent>
{
    private readonly ILogger<BusRegisteredEventHandler> _logger;

    public BusRegisteredEventHandler(ILogger<BusRegisteredEventHandler> logger) => _logger = logger;

    public Task Handle(BusRegisteredEvent notification, CancellationToken ct)
    {
        _logger.LogInformation(
            "Bus registered: Id={BusId}, RegNumber={RegNumber}, By={RegisteredBy}",
            notification.BusId, notification.RegistrationNumber, notification.RegisteredBy);
        return Task.CompletedTask;
    }
}

public sealed class EmployeeAssignedToBusEventHandler : INotificationHandler<EmployeeAssignedToBusEvent>
{
    private readonly ILogger<EmployeeAssignedToBusEventHandler> _logger;

    public EmployeeAssignedToBusEventHandler(ILogger<EmployeeAssignedToBusEventHandler> logger) => _logger = logger;

    public Task Handle(EmployeeAssignedToBusEvent notification, CancellationToken ct)
    {
        _logger.LogInformation(
            "Employee {EmpSysId} assigned to Bus {BusId} on Route {RouteId}, AssignmentId={EmpBusId}",
            notification.EmpSysId, notification.BusId, notification.RouteId, notification.EmpBusId);
        return Task.CompletedTask;
    }
}

public sealed class BusArrivedEventHandler : INotificationHandler<BusArrivedEvent>
{
    private readonly ILogger<BusArrivedEventHandler> _logger;

    public BusArrivedEventHandler(ILogger<BusArrivedEventHandler> logger) => _logger = logger;

    public Task Handle(BusArrivedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation(
            "Bus {BusId} arrival recorded: ArrivalId={ArrivalId}, Date={Date}, Status={Status}",
            notification.BusId, notification.ArrivalId, notification.ArrivalDate, notification.Status);
        return Task.CompletedTask;
    }
}
