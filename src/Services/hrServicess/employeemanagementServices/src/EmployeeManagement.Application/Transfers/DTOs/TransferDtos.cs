namespace EmployeeManagement.Application.Transfers.DTOs;

public sealed record TransferDto(
    long TransferId,
    long EmployeeId,
    string? OldUnit,
    string? NewUnit,
    long OldUnitId,
    long NewUnitId,
    DateTime TransferDate,
    string? Remarks,
    string TransferType,
    string Status,
    long? CreatedBy,
    DateTime? CreatedOn
);
