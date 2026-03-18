using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.MedicineIssues.Commands;

public record CreateMedicineIssueCommand(
    string CompanyCode, string TransactionNumber, string MedicineCode,
    long IssuedQuantity, string VisitNumber,
    string EntryUser, string EntryUserPin) : IRequest<MedicineIssueDto>;
