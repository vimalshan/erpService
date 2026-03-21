using MediatR;
using RackingSystem.Application.Features.Racks.Commands.CreateRack;
using RackingSystem.Application.Features.Racks.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Racks.Queries.GetRackById;

public record GetRackByIdQuery(int Id) : IRequest<RackDto>;

public sealed class GetRackByIdQueryHandler : IRequestHandler<GetRackByIdQuery, RackDto>
{
    private readonly IUnitOfWork _uow;

    public GetRackByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RackDto> Handle(GetRackByIdQuery request, CancellationToken ct)
    {
        var rack = await _uow.Racks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Rack), request.Id);
        return CreateRackCommandHandler.MapToDto(rack);
    }
}
