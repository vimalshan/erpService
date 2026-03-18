using MediatR;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAllAbsenteeismDetails;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetailByPeriod;
using TimeAttendance.Application.AbsenteeismMis.Queries.GetAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Queries.GetAllAbsenteeismMis;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.API.GraphQL.Queries;

public class Query
{
    [GraphQLDescription("Get all absenteeism detail records.")]
    public async Task<PaginatedResult<AbsenteeismDetailDto>> GetAbsenteeismDetails(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 20,
        long? unitId = null,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllAbsenteeismDetailsQuery(pageNumber, pageSize, unitId), cancellationToken);

    [GraphQLDescription("Get absenteeism detail by ID.")]
    public async Task<AbsenteeismDetailDto?> GetAbsenteeismDetailById(
        [Service] IMediator mediator,
        long id,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAbsenteeismDetailQuery(id), cancellationToken);

    [GraphQLDescription("Get absenteeism details by unit and period.")]
    public async Task<IEnumerable<AbsenteeismDetailDto>> GetAbsenteeismDetailsByPeriod(
        [Service] IMediator mediator,
        long unitId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAbsenteeismDetailByPeriodQuery(unitId, year, month), cancellationToken);

    [GraphQLDescription("Get all absenteeism MIS records.")]
    public async Task<PaginatedResult<AbsenteeismMisDto>> GetAbsenteeismMisRecords(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 20,
        int? unitId = null,
        string? month = null,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllAbsenteeismMisQuery(pageNumber, pageSize, unitId, month), cancellationToken);

    [GraphQLDescription("Get absenteeism MIS by ID.")]
    public async Task<AbsenteeismMisDto?> GetAbsenteeismMisById(
        [Service] IMediator mediator,
        long id,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAbsenteeismMisQuery(id), cancellationToken);
}
