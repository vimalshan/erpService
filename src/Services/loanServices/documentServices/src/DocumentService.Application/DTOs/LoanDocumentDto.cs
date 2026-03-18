namespace DocumentService.Application.DTOs;

public record LoanDocumentDto(
    long Id,
    long LoanId,
    long TypeId,
    long LastModifiedBy,
    DateTime LastModifiedOn);
