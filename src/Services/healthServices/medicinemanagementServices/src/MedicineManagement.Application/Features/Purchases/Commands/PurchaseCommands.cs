using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.Purchases.Commands;

public record CreatePurchaseCommand(
    string CompanyCode, long TransactionNumber, string VendorName,
    string InvoiceNumber, DateTime InvoiceDate, decimal InvoiceAmount,
    string EntryUser, decimal EntryUserPin,
    List<CreatePurchaseLineItemDto> LineItems) : IRequest<PurchaseMainDto>;

public record CancelPurchaseCommand(string CompanyCode, long TransactionNumber, string ModifiedUser, decimal ModifiedUserPin) : IRequest<bool>;
