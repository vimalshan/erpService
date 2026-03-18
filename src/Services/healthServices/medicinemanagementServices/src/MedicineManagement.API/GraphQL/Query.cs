using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineCredits.Queries;
using MedicineManagement.Application.Features.MedicineIssues.Queries;
using MedicineManagement.Application.Features.Medicines.Queries;
using MedicineManagement.Application.Features.MedicineTypes.Queries;
using MedicineManagement.Application.Features.Purchases.Queries;

namespace MedicineManagement.API.GraphQL;

public class Query
{
    // Medicine Types
    public async Task<IReadOnlyList<MedicineTypeDto>> GetMedicineTypes([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllMedicineTypesQuery(), ct);

    public async Task<MedicineTypeDto?> GetMedicineType([Service] IMediator mediator, string typeCode, CancellationToken ct)
        => await mediator.Send(new GetMedicineTypeByCodeQuery(typeCode), ct);

    // Medicines
    public async Task<IReadOnlyList<MedicineDto>> GetMedicines([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllMedicinesQuery(), ct);

    public async Task<MedicineDto?> GetMedicine([Service] IMediator mediator, string medicineCode, CancellationToken ct)
        => await mediator.Send(new GetMedicineByCodeQuery(medicineCode), ct);

    public async Task<IReadOnlyList<MedicineDto>> SearchMedicines([Service] IMediator mediator, string name, CancellationToken ct)
        => await mediator.Send(new SearchMedicinesQuery(name), ct);

    public async Task<IReadOnlyList<StockSummaryDto>> GetLowStockMedicines([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLowStockMedicinesQuery(), ct);

    // Stock
    public async Task<long> GetStockBalance([Service] IMediator mediator, string medicineCode, CancellationToken ct)
        => await mediator.Send(new GetStockByMedicineQuery(medicineCode), ct);

    public async Task<IReadOnlyList<MedicineCreditDto>> GetTransactions([Service] IMediator mediator, string medicineCode, CancellationToken ct)
        => await mediator.Send(new GetTransactionsByMedicineQuery(medicineCode), ct);

    // Purchases
    public async Task<PurchaseMainDto?> GetPurchase([Service] IMediator mediator, string companyCode, long transactionNumber, CancellationToken ct)
        => await mediator.Send(new GetPurchaseByIdQuery(companyCode, transactionNumber), ct);

    public async Task<IReadOnlyList<PurchaseMainDto>> GetPurchasesByDateRange([Service] IMediator mediator, DateTime from, DateTime to, CancellationToken ct)
        => await mediator.Send(new GetPurchasesByDateRangeQuery(from, to), ct);

    // Issues
    public async Task<IReadOnlyList<MedicineIssueDto>> GetIssuesByVisit([Service] IMediator mediator, string visitNumber, CancellationToken ct)
        => await mediator.Send(new GetIssuesByVisitQuery(visitNumber), ct);
}
