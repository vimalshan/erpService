using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Queries.Drivers;

public record GetDriverByIdQuery(int DriverId) : IRequest<DriverDto?>;
public record GetAllDriversQuery : IRequest<IReadOnlyList<DriverDto>>;
public record GetActiveDriversQuery : IRequest<IReadOnlyList<DriverDto>>;

public class GetDriverByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetDriverByIdQuery, DriverDto?>
{
    public async Task<DriverDto?> Handle(GetDriverByIdQuery request, CancellationToken ct)
    {
        var driver = await uow.Drivers.GetByIdAsync(request.DriverId, ct);
        return driver is null ? null : mapper.Map<DriverDto>(driver);
    }
}

public class GetAllDriversHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllDriversQuery, IReadOnlyList<DriverDto>>
{
    public async Task<IReadOnlyList<DriverDto>> Handle(GetAllDriversQuery request, CancellationToken ct)
    {
        var drivers = await uow.Drivers.GetAllAsync(ct);
        return drivers.Select(mapper.Map<DriverDto>).ToList();
    }
}

public class GetActiveDriversHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetActiveDriversQuery, IReadOnlyList<DriverDto>>
{
    public async Task<IReadOnlyList<DriverDto>> Handle(GetActiveDriversQuery request, CancellationToken ct)
    {
        var drivers = await uow.Drivers.GetActiveDriversAsync(ct);
        return drivers.Select(mapper.Map<DriverDto>).ToList();
    }
}
