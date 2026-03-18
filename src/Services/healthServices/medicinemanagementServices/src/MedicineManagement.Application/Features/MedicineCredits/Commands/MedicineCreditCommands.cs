using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.MedicineCredits.Commands;

public record CreateMedicineCreditCommand(
    string CompanyCode, long TransactionCode, string MedicineCode,
    char RecordType, long Quantity, DateTime TransactionDate,
    string EntryUser, decimal EntryUserPin, string? LotNumber) : IRequest<MedicineCreditDto>;
