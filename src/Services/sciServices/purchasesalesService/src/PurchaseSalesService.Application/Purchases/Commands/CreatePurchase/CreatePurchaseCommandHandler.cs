using MediatR;
using Microsoft.Extensions.Logging;
using PurchaseSalesService.Application.Common.Interfaces;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Domain.Entities;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;

public sealed class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, PurchaseDetailDto>
{
    private readonly IPurchaseRepository _repo;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CreatePurchaseCommandHandler> _logger;

    public CreatePurchaseCommandHandler(
        IPurchaseRepository repo,
        IMessagePublisher publisher,
        ILogger<CreatePurchaseCommandHandler> logger)
    {
        _repo = repo;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<PurchaseDetailDto> Handle(CreatePurchaseCommand command, CancellationToken ct)
    {
        var purchase = PurchaseDetail.Create(
            command.TrackingNumber,
            transactionNumber: 1,
            command.PurposeCode,
            command.StageCode,
            command.SupplierCode,
            command.UserId,
            command.UserNumber);

        await _repo.AddAsync(purchase, ct);
        await _repo.SaveChangesAsync(ct);

        try
        {
            await _publisher.PublishAsync("purchase.created", new
            {
                purchase.SerialNumber,
                purchase.TrackingNumber,
                purchase.UserId,
                purchase.UpdatedAt
            }, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not publish purchase.created event for TrackingNumber={TN}. Message will not be retried.", purchase.TrackingNumber);
        }

        return MapToDto(purchase);
    }

    internal static PurchaseDetailDto MapToDto(PurchaseDetail p) => new(
        p.SerialNumber, p.TrackingNumber, p.TransactionNumber,
        p.PurposeCode, p.StageCode, p.OracleMerchandise,
        p.SupplierCode, p.TonNumLoaded, p.TonNumUnloaded,
        p.UserId, p.UserNumber, p.UpdatedAt, p.CancelFlag?.ToString());
}
