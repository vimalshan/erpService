using MediatR;
using Microsoft.Extensions.Logging;
using PurchaseSalesService.Application.Common.Interfaces;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Domain.Entities;
using PurchaseSalesService.Domain.Interfaces;

namespace PurchaseSalesService.Application.Sales.Commands.CreateSale;

public sealed class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleMainDto>
{
    private readonly ISaleRepository _repo;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CreateSaleCommandHandler> _logger;

    public CreateSaleCommandHandler(
        ISaleRepository repo,
        IMessagePublisher publisher,
        ILogger<CreateSaleCommandHandler> logger)
    {
        _repo = repo;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<SaleMainDto> Handle(CreateSaleCommand command, CancellationToken ct)
    {
        var sale = SaleMain.Create(
            command.TrackingNumber,
            transactionNumber: 1,
            command.PurposeCode,
            command.StageCode,
            command.UserId,
            command.UserNumber,
            command.VehicleCustomer);

        await _repo.AddAsync(sale, ct);
        await _repo.SaveChangesAsync(ct);

        try
        {
            await _publisher.PublishAsync("sale.created", new
            {
                sale.SerialNumber,
                sale.TrackingNumber,
                sale.UserId,
                sale.UpdatedAt
            }, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not publish sale.created event for TrackingNumber={TN}. Message will not be retried.", sale.TrackingNumber);
        }

        return MapToDto(sale);
    }

    internal static SaleMainDto MapToDto(SaleMain s) => new(
        s.SerialNumber, s.TrackingNumber, s.TransactionNumber,
        s.PurposeCode, s.StageCode, s.IsoNumber, s.IsoDate,
        s.ProductDescription, s.UserId, s.UserNumber,
        s.UpdatedAt, s.CancelFlag, s.VehicleCustomer,
        s.SaleSubItems.Select(sub => new SaleSubDto(
            sub.ReferenceNumber, sub.SerialNumber, sub.ProductCode,
            sub.ProductQuantity, sub.ProductGrade, sub.UserComment,
            sub.CheckbookInvoice, sub.CancelFlag)).ToList().AsReadOnly());
}
