using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Entities;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.EmployeeAssignments.Commands;

// ─── Assign Employee To Bus ───────────────────────────────────────────────────

public record AssignEmployeeToBusCommand(
    long EmpSysId,
    int BusId,
    int RouteId,
    long AssignedBy) : IRequest<EmployeeBusDto>;

public sealed class AssignEmployeeToBusCommandHandler : IRequestHandler<AssignEmployeeToBusCommand, EmployeeBusDto>
{
    private readonly IBusRepository _busRepo;
    private readonly IBusRouteRepository _routeRepo;
    private readonly IEmployeeBusRepository _empBusRepo;

    public AssignEmployeeToBusCommandHandler(
        IBusRepository busRepo,
        IBusRouteRepository routeRepo,
        IEmployeeBusRepository empBusRepo)
    {
        _busRepo = busRepo;
        _routeRepo = routeRepo;
        _empBusRepo = empBusRepo;
    }

    public async Task<EmployeeBusDto> Handle(AssignEmployeeToBusCommand request, CancellationToken ct)
    {
        if (!await _busRepo.ExistsAsync(request.BusId, ct))
            throw new KeyNotFoundException($"Bus {request.BusId} not found.");

        if (!await _routeRepo.ExistsForBusAsync(request.RouteId, request.BusId, ct))
            throw new KeyNotFoundException($"Route {request.RouteId} not found for bus {request.BusId}.");

        long nextId = await _empBusRepo.GetNextIdAsync(ct);
        var assignment = EmployeeBus.Assign(nextId, request.EmpSysId, request.BusId, request.RouteId, request.AssignedBy);

        await _empBusRepo.AddAsync(assignment, ct);
        await _empBusRepo.SaveChangesAsync(ct);

        return new EmployeeBusDto(
            assignment.EmpBusId, assignment.EmpSysId, assignment.BusId,
            assignment.RouteId, assignment.EffectiveDate, assignment.ClosingDate,
            assignment.LastModifiedBy, assignment.LastModifiedOn);
    }
}

// ─── Close Employee Assignment ────────────────────────────────────────────────

public record CloseEmployeeAssignmentCommand(long EmpBusId, DateTime ClosingDate, long ModifiedBy) : IRequest<EmployeeBusDto>;

public sealed class CloseEmployeeAssignmentCommandHandler : IRequestHandler<CloseEmployeeAssignmentCommand, EmployeeBusDto>
{
    private readonly IEmployeeBusRepository _repo;

    public CloseEmployeeAssignmentCommandHandler(IEmployeeBusRepository repo) => _repo = repo;

    public async Task<EmployeeBusDto> Handle(CloseEmployeeAssignmentCommand request, CancellationToken ct)
    {
        var assignment = await _repo.GetByIdAsync(request.EmpBusId, ct)
            ?? throw new KeyNotFoundException($"Employee bus assignment {request.EmpBusId} not found.");

        assignment.Close(request.ClosingDate, request.ModifiedBy);
        _repo.Update(assignment);
        await _repo.SaveChangesAsync(ct);

        return new EmployeeBusDto(
            assignment.EmpBusId, assignment.EmpSysId, assignment.BusId,
            assignment.RouteId, assignment.EffectiveDate, assignment.ClosingDate,
            assignment.LastModifiedBy, assignment.LastModifiedOn);
    }
}
