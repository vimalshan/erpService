using MedicalVisit.Application.Common.Interfaces;
using MedicalVisit.Application.Common.Models;
using MedicalVisit.Application.DTOs;

namespace MedicalVisit.Application.Visits.Queries.GetVisitsByDateRange;

public record GetVisitsByDateRangeQuery : IQuery<Result<List<VisitDto>>>
{
    public string CompanyCode { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
