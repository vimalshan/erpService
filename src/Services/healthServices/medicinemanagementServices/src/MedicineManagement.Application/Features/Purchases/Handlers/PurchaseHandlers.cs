using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Purchases.Commands;
using MedicineManagement.Application.Features.Purchases.Queries;
using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.Purchases.Handlers;

public class GetPurchaseByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPurchaseByIdQuery, PurchaseMainDto?>
{
    public async Task<PurchaseMainDto?> Handle(GetPurchaseByIdQuery request, CancellationToken ct)
    {
        var purchase = await unitOfWork.Purchases.GetByIdAsync(request.CompanyCode, request.TransactionNumber, ct);
        return purchase is null ? null : mapper.Map<PurchaseMainDto>(purchase);
    }
}

public class GetPurchasesByDateRangeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPurchasesByDateRangeQuery, IReadOnlyList<PurchaseMainDto>>
{
    public async Task<IReadOnlyList<PurchaseMainDto>> Handle(GetPurchasesByDateRangeQuery request, CancellationToken ct)
    {
        var purchases = await unitOfWork.Purchases.GetByDateRangeAsync(request.From, request.To, ct);
        return mapper.Map<IReadOnlyList<PurchaseMainDto>>(purchases);
    }
}

public class GetPurchasesByVendorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPurchasesByVendorQuery, IReadOnlyList<PurchaseMainDto>>
{
    public async Task<IReadOnlyList<PurchaseMainDto>> Handle(GetPurchasesByVendorQuery request, CancellationToken ct)
    {
        var purchases = await unitOfWork.Purchases.GetByVendorAsync(request.VendorName, ct);
        return mapper.Map<IReadOnlyList<PurchaseMainDto>>(purchases);
    }
}

public class CreatePurchaseHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreatePurchaseCommand, PurchaseMainDto>
{
    public async Task<PurchaseMainDto> Handle(CreatePurchaseCommand request, CancellationToken ct)
    {
        var purchase = PurchaseMain.Create(
            request.CompanyCode, request.TransactionNumber, request.VendorName,
            request.InvoiceNumber, request.InvoiceDate, request.InvoiceAmount,
            request.EntryUser, request.EntryUserPin);

        foreach (var item in request.LineItems)
        {
            var sub = PurchaseSub.Create(
                request.CompanyCode, request.TransactionNumber, item.SerialNumber,
                item.MedicineCode, item.PackagingType, item.PackagingQuantity,
                item.PackagingNos, item.TotalQuantity,
                item.ManufacturingDate, item.ExpiryDate, item.LotNumber,
                request.EntryUser, request.EntryUserPin);
            purchase.AddLineItem(sub);

            // Create stock credit record for each line item
            var credit = MedicineCredit.Create(
                request.CompanyCode, request.TransactionNumber, item.MedicineCode,
                'P', item.TotalQuantity ?? 0, DateTime.UtcNow,
                request.EntryUser, request.EntryUserPin, item.LotNumber);
            await unitOfWork.MedicineCredits.AddAsync(credit, ct);
        }

        await unitOfWork.Purchases.AddAsync(purchase, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<PurchaseMainDto>(purchase);
    }
}

public class CancelPurchaseHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CancelPurchaseCommand, bool>
{
    public async Task<bool> Handle(CancelPurchaseCommand request, CancellationToken ct)
    {
        var purchase = await unitOfWork.Purchases.GetByIdAsync(request.CompanyCode, request.TransactionNumber, ct)
            ?? throw new KeyNotFoundException("Purchase not found.");
        purchase.Cancel(request.ModifiedUser, request.ModifiedUserPin);
        await unitOfWork.Purchases.UpdateAsync(purchase, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
