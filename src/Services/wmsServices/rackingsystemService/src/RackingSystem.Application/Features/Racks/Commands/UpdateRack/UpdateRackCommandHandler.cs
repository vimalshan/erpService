using MediatR;
using RackingSystem.Application.Features.Racks.Commands.CreateRack;
using RackingSystem.Application.Features.Racks.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Racks.Commands.UpdateRack;

public sealed class UpdateRackCommandHandler : IRequestHandler<UpdateRackCommand, RackDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateRackCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RackDto> Handle(UpdateRackCommand request, CancellationToken ct)
    {
        var rack = await _uow.Racks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Rack), request.Id);

        rack.Update(request.Code, request.RackType, request.MaxLoadWeight);
        _uow.Racks.Update(rack);
        await _uow.SaveChangesAsync(ct);

        return CreateRackCommandHandler.MapToDto(rack);
    }
}
