using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineCredits.Commands;
using MedicineManagement.Application.Features.MedicineIssues.Commands;
using MedicineManagement.Application.Features.Medicines.Commands;
using MedicineManagement.Application.Features.MedicineTypes.Commands;
using MedicineManagement.Application.Features.Purchases.Commands;

namespace MedicineManagement.API.GraphQL;

public class Mutation
{
    // Medicine Types
    public async Task<MedicineTypeDto> CreateMedicineType(
        [Service] IMediator mediator, string typeCode, string? typeName, CancellationToken ct)
        => await mediator.Send(new CreateMedicineTypeCommand(typeCode, typeName, "GraphQL", null), ct);

    public async Task<MedicineTypeDto> UpdateMedicineType(
        [Service] IMediator mediator, string typeCode, string? typeName, CancellationToken ct)
        => await mediator.Send(new UpdateMedicineTypeCommand(typeCode, typeName, "GraphQL", null), ct);

    public async Task<bool> DeleteMedicineType(
        [Service] IMediator mediator, string typeCode, CancellationToken ct)
        => await mediator.Send(new DeleteMedicineTypeCommand(typeCode), ct);

    // Medicines
    public async Task<MedicineDto> CreateMedicine(
        [Service] IMediator mediator,
        string medicineCode, string medicineName, string medicineTypeCode,
        char? category, decimal? orderLevelMin, decimal? orderLevelMax, CancellationToken ct)
        => await mediator.Send(new CreateMedicineCommand(
            medicineCode, medicineName, medicineTypeCode, category, orderLevelMin, orderLevelMax, "GraphQL", null), ct);

    public async Task<MedicineDto> UpdateMedicine(
        [Service] IMediator mediator,
        string medicineCode, string medicineName, string medicineTypeCode,
        char? category, decimal? orderLevelMin, decimal? orderLevelMax, CancellationToken ct)
        => await mediator.Send(new UpdateMedicineCommand(
            medicineCode, medicineName, medicineTypeCode, category, orderLevelMin, orderLevelMax, "GraphQL", null), ct);

    public async Task<bool> DeleteMedicine(
        [Service] IMediator mediator, string medicineCode, CancellationToken ct)
        => await mediator.Send(new DeleteMedicineCommand(medicineCode), ct);

    // Purchases
    public async Task<PurchaseMainDto> CreatePurchase(
        [Service] IMediator mediator,
        string companyCode, long transactionNumber, string vendorName,
        string invoiceNumber, DateTime invoiceDate, decimal invoiceAmount,
        List<CreatePurchaseLineItemDto> lineItems, CancellationToken ct)
        => await mediator.Send(new CreatePurchaseCommand(
            companyCode, transactionNumber, vendorName, invoiceNumber, invoiceDate, invoiceAmount,
            "GraphQL", 0, lineItems), ct);

    public async Task<bool> CancelPurchase(
        [Service] IMediator mediator, string companyCode, long transactionNumber, CancellationToken ct)
        => await mediator.Send(new CancelPurchaseCommand(companyCode, transactionNumber, "GraphQL", 0), ct);

    // Stock
    public async Task<MedicineCreditDto> CreateStockTransaction(
        [Service] IMediator mediator,
        string companyCode, long transactionCode, string medicineCode,
        char recordType, long quantity, DateTime transactionDate,
        string? lotNumber, CancellationToken ct)
        => await mediator.Send(new CreateMedicineCreditCommand(
            companyCode, transactionCode, medicineCode, recordType, quantity, transactionDate,
            "GraphQL", 0, lotNumber), ct);

    // Issue
    public async Task<MedicineIssueDto> IssueMedicine(
        [Service] IMediator mediator,
        string companyCode, string transactionNumber, string medicineCode,
        long issuedQuantity, string visitNumber, CancellationToken ct)
        => await mediator.Send(new CreateMedicineIssueCommand(
            companyCode, transactionNumber, medicineCode, issuedQuantity, visitNumber,
            "GraphQL", "0"), ct);
}
