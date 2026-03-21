using MediatR;
using RackingSystem.Application.Features.Bins.Commands;
using RackingSystem.Application.Features.Bins.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Exceptions;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Bins.Queries;

public record GetBinsQuery(int? ZoneId = null, int? ShelfId = null, string? Status = null) : IRequest<IEnumerable<BinDto>>;

public sealed class GetBinsQueryHandler : IRequestHandler<GetBinsQuery, IEnumerable<BinDto>>
{
    private readonly IUnitOfWork _uow;
    public GetBinsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<BinDto>> Handle(GetBinsQuery request, CancellationToken ct)
    {
        var bins = request.ZoneId.HasValue
            ? await _uow.Bins.GetByZoneIdAsync(request.ZoneId.Value, ct)
            : request.ShelfId.HasValue
                ? await _uow.Bins.GetByShelfIdAsync(request.ShelfId.Value, ct)
                : request.Status != null
                    ? await _uow.Bins.GetByStatusAsync(request.Status, ct)
                    : await _uow.Bins.GetAllAsync(ct);

        return bins.Select(b => CreateBinCommandHandler.MapToDto(b, null));
    }
}

public record GetBinByIdQuery(int Id) : IRequest<BinDto>;

public sealed class GetBinByIdQueryHandler : IRequestHandler<GetBinByIdQuery, BinDto>
{
    private readonly IUnitOfWork _uow;
    public GetBinByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<BinDto> Handle(GetBinByIdQuery request, CancellationToken ct)
    {
        var bin = await _uow.Bins.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Bin), request.Id);

        var utilization = await _uow.Bins.GetBinUtilizationAsync(request.Id, ct);
        return CreateBinCommandHandler.MapToDto(bin, utilization);
    }
}

public record GetBinByBarcodeQuery(string Barcode) : IRequest<BinDto>;

public sealed class GetBinByBarcodeQueryHandler : IRequestHandler<GetBinByBarcodeQuery, BinDto>
{
    private readonly IUnitOfWork _uow;
    public GetBinByBarcodeQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<BinDto> Handle(GetBinByBarcodeQuery request, CancellationToken ct)
    {
        var bin = await _uow.Bins.GetByBarcodeAsync(request.Barcode, ct)
            ?? throw new NotFoundException("Bin", $"barcode={request.Barcode}");
        return CreateBinCommandHandler.MapToDto(bin, null);
    }
}
