using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Application.Common.Models;
using MedicalVisit.Application.DTOs;

namespace MedicalVisit.Application.Visits.Queries.GetVisitById;

public record GetVisitByIdQuery : IQuery<Result<VisitDto>>
{
    public string CompanyCode { get; init; } = string.Empty;
    public long VisitNumber { get; init; }
}
