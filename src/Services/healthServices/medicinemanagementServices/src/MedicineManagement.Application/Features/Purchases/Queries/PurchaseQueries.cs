using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.Purchases.Queries;

public record GetPurchaseByIdQuery(string CompanyCode, long TransactionNumber) : IRequest<PurchaseMainDto?>;
public record GetPurchasesByDateRangeQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<PurchaseMainDto>>;
public record GetPurchasesByVendorQuery(string VendorName) : IRequest<IReadOnlyList<PurchaseMainDto>>;
