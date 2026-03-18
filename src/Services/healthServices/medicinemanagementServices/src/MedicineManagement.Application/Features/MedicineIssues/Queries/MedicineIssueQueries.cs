using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.MedicineIssues.Queries;

public record GetIssuesByVisitQuery(string VisitNumber) : IRequest<IReadOnlyList<MedicineIssueDto>>;
public record GetIssuesByMedicineQuery(string MedicineCode) : IRequest<IReadOnlyList<MedicineIssueDto>>;
