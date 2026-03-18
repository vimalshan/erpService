using MediatR;
using MedicalVisit.Application.DTOs;
using MedicalVisit.Application.Visits.Queries.GetVisitById;
using MedicalVisit.Application.Visits.Queries.GetVisitsByDateRange;

namespace MedicalVisit.API.GraphQL;

public class VisitQuery
{
    public async Task<VisitDto?> GetVisitAsync(
        string companyCode,
        long visitNumber,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVisitByIdQuery { CompanyCode = companyCode, VisitNumber = visitNumber };
        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? result.Data : null;
    }

    public async Task<List<VisitDto>?> GetVisitsByDateRangeAsync(
        string companyCode,
        DateTime startDate,
        DateTime endDate,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVisitsByDateRangeQuery
        {
            CompanyCode = companyCode,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await mediator.Send(query, cancellationToken);

        return result.IsSuccess ? result.Data : null;
    }
}
