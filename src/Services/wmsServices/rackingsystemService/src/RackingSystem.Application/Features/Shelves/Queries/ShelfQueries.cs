using MediatR;
using RackingSystem.Application.Features.Shelves.Commands;
using RackingSystem.Application.Features.Shelves.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Shelves.Queries;

public record GetShelvesByRackQuery(int RackId) : IRequest<IEnumerable<ShelfDto>>;

public sealed class GetShelvesByRackQueryHandler : IRequestHandler<GetShelvesByRackQuery, IEnumerable<ShelfDto>>
{
    private readonly IUnitOfWork _uow;
    public GetShelvesByRackQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<ShelfDto>> Handle(GetShelvesByRackQuery request, CancellationToken ct)
    {
        var shelves = await _uow.Shelves.GetByRackIdAsync(request.RackId, ct);
        return shelves.Select(CreateShelfCommandHandler.MapToDto);
    }
}

public record GetShelfByIdQuery(int Id) : IRequest<ShelfDto>;

public sealed class GetShelfByIdQueryHandler : IRequestHandler<GetShelfByIdQuery, ShelfDto>
{
    private readonly IUnitOfWork _uow;
    public GetShelfByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ShelfDto> Handle(GetShelfByIdQuery request, CancellationToken ct)
    {
        var shelf = await _uow.Shelves.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Shelf), request.Id);
        return CreateShelfCommandHandler.MapToDto(shelf);
    }
}
