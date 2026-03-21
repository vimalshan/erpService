using FluentValidation;
using MediatR;
using RackingSystem.Application.Features.Shelves.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Shelves.Commands;

// ---- Create ----
public record CreateShelfCommand(
    int RackId, int ShelfLevel, int ShelfPosition, string Code,
    decimal? CapacityQty, decimal? CapacityWeight
) : IRequest<ShelfDto>;

public sealed class CreateShelfCommandValidator : AbstractValidator<CreateShelfCommand>
{
    public CreateShelfCommandValidator()
    {
        RuleFor(x => x.RackId).GreaterThan(0);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.ShelfLevel).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ShelfPosition).GreaterThanOrEqualTo(0);
    }
}

public sealed class CreateShelfCommandHandler : IRequestHandler<CreateShelfCommand, ShelfDto>
{
    private readonly IUnitOfWork _uow;
    public CreateShelfCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ShelfDto> Handle(CreateShelfCommand request, CancellationToken ct)
    {
        var rack = await _uow.Racks.GetByIdAsync(request.RackId, ct)
            ?? throw new NotFoundException(nameof(Rack), request.RackId);

        var shelf = Shelf.Create(request.RackId, request.ShelfLevel, request.ShelfPosition,
            request.Code, request.CapacityQty, request.CapacityWeight);

        await _uow.Shelves.AddAsync(shelf, ct);
        await _uow.SaveChangesAsync(ct);
        return MapToDto(shelf);
    }

    internal static ShelfDto MapToDto(Shelf s) => new(
        s.Id, s.RackId, s.ShelfLevel, s.ShelfPosition, s.Code,
        s.CapacityQty, s.CapacityWeight, s.IsActive, s.CreatedDate, s.ModifiedDate);
}

// ---- Update ----
public record UpdateShelfCommand(int Id, string Code, decimal? CapacityQty, decimal? CapacityWeight) : IRequest<ShelfDto>;

public sealed class UpdateShelfCommandHandler : IRequestHandler<UpdateShelfCommand, ShelfDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateShelfCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ShelfDto> Handle(UpdateShelfCommand request, CancellationToken ct)
    {
        var shelf = await _uow.Shelves.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Shelf), request.Id);

        shelf.Update(request.Code, request.CapacityQty, request.CapacityWeight);
        _uow.Shelves.Update(shelf);
        await _uow.SaveChangesAsync(ct);
        return CreateShelfCommandHandler.MapToDto(shelf);
    }
}

// ---- Delete ----
public record DeleteShelfCommand(int Id) : IRequest;

public sealed class DeleteShelfCommandHandler : IRequestHandler<DeleteShelfCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteShelfCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteShelfCommand request, CancellationToken ct)
    {
        var shelf = await _uow.Shelves.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Shelf), request.Id);
        shelf.Deactivate();
        _uow.Shelves.Update(shelf);
        await _uow.SaveChangesAsync(ct);
    }
}
