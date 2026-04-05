using AuditService.Application.DTOs;
using AuditService.Application.Queries.Audits;
using AuditService.Application.Queries.Observations;
using AuditService.Application.Queries.GoodPractices;
using AuditService.Infrastructure.Dapper;
using MediatR;

namespace AuditService.API.GraphQL;

public class AuditGraphQLQuery
{
    public async Task<IEnumerable<AuditDto>> GetAudits([Service] ISender sender, CancellationToken cancellationToken)
        => await sender.Send(new GetAllAuditsQuery(), cancellationToken);

    public async Task<AuditDto?> GetAuditById([Service] ISender sender, long id, CancellationToken cancellationToken)
        => await sender.Send(new GetAuditByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<AuditDto>> GetAuditsByUnit([Service] ISender sender, long unitId, CancellationToken cancellationToken)
        => await sender.Send(new GetAuditsByUnitQuery(unitId), cancellationToken);

    public async Task<IEnumerable<ObservationDto>> GetObservationsByAudit([Service] ISender sender, long auditId, CancellationToken cancellationToken)
        => await sender.Send(new GetObservationsByAuditQuery(auditId), cancellationToken);

    public async Task<IEnumerable<ObservationDto>> GetPendingObservations([Service] ISender sender, CancellationToken cancellationToken)
        => await sender.Send(new GetPendingObservationsQuery(), cancellationToken);

    public async Task<IEnumerable<GoodPracticeDto>> GetGoodPractices([Service] ISender sender, CancellationToken cancellationToken)
        => await sender.Send(new GetAllGoodPracticesQuery(), cancellationToken);

    [GraphQLIgnore]
    public async Task<IEnumerable<dynamic>> GetAuditSummary([Service] AuditDapperRepository dapper, int year, CancellationToken cancellationToken)
        => (await dapper.GetAuditSummaryAsync(year, cancellationToken)).Cast<dynamic>();

    [GraphQLIgnore]
    public async Task<IEnumerable<dynamic>> GetOverdueObservations([Service] AuditDapperRepository dapper, CancellationToken cancellationToken)
        => (await dapper.GetOverdueObservationsAsync(cancellationToken)).Cast<dynamic>();
}
