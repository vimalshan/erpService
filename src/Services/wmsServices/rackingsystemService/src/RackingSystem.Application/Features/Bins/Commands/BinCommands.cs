using FluentValidation;
using MediatR;
using RackingSystem.Application.Features.Bins.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Bins.Commands;

// ---- Create ----
public record CreateBinCommand(
    int ZoneId, string Code, int? ShelfId, string? Barcode, string? BinType,
    decimal? CapacityQty, decimal? CapacityWeight, decimal? CapacityVolume
) : IRequest<BinDto>;

public sealed class CreateBinCommandValidator : AbstractValidator<CreateBinCommand>
{
    public CreateBinCommandValidator()
    {
        RuleFor(x => x.ZoneId).GreaterThan(0);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Barcode).MaximumLength(50).When(x => x.Barcode != null);
    }
}

public sealed class CreateBinCommandHandler : IRequestHandler<CreateBinCommand, BinDto>
{
    private readonly IUnitOfWork _uow;
    public CreateBinCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<BinDto> Handle(CreateBinCommand request, CancellationToken ct)
    {
        var bin = Bin.Create(request.ZoneId, request.Code, request.ShelfId,
            request.Barcode, request.BinType,
            request.CapacityQty, request.CapacityWeight, request.CapacityVolume);

        await _uow.Bins.AddAsync(bin, ct);
        await _uow.SaveChangesAsync(ct);
        return MapToDto(bin, null);
    }

    internal static BinDto MapToDto(Bin b, decimal? utilization) => new(
        b.Id, b.ZoneId, b.ShelfId, b.Code, b.Barcode, b.BinType,
        b.CapacityQty, b.CapacityWeight, b.CapacityVolume,
        b.Status, b.IsActive, utilization, b.CreatedDate, b.ModifiedDate);
}

// ---- Update Status ----
public record UpdateBinStatusCommand(int Id, string NewStatus) : IRequest<BinDto>;

public sealed class UpdateBinStatusCommandHandler : IRequestHandler<UpdateBinStatusCommand, BinDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateBinStatusCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<BinDto> Handle(UpdateBinStatusCommand request, CancellationToken ct)
    {
        var bin = await _uow.Bins.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Bin), request.Id);

        bin.UpdateStatus(request.NewStatus);
        _uow.Bins.Update(bin);
        await _uow.SaveChangesAsync(ct);
        return CreateBinCommandHandler.MapToDto(bin, null);
    }
}

// ---- Update ----
public record UpdateBinCommand(
    int Id, string Code, string? Barcode, string? BinType,
    decimal? CapacityQty, decimal? CapacityWeight, decimal? CapacityVolume
) : IRequest<BinDto>;

public sealed class UpdateBinCommandHandler : IRequestHandler<UpdateBinCommand, BinDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateBinCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<BinDto> Handle(UpdateBinCommand request, CancellationToken ct)
    {
        var bin = await _uow.Bins.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Bin), request.Id);

        bin.Update(request.Code, request.Barcode, request.BinType,
            request.CapacityQty, request.CapacityWeight, request.CapacityVolume);
        _uow.Bins.Update(bin);
        await _uow.SaveChangesAsync(ct);
        return CreateBinCommandHandler.MapToDto(bin, null);
    }
}

// ---- Delete ----
public record DeleteBinCommand(int Id) : IRequest;

public sealed class DeleteBinCommandHandler : IRequestHandler<DeleteBinCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteBinCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteBinCommand request, CancellationToken ct)
    {
        var bin = await _uow.Bins.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Bin), request.Id);
        bin.Deactivate();
        _uow.Bins.Update(bin);
        await _uow.SaveChangesAsync(ct);
    }
}
