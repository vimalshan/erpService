using MediatR;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Application.Queries.GetScholarshipAmounts;
using ScholarshipService.Application.Queries.GetScholarshipById;
using ScholarshipService.Application.Queries.GetScholarships;

namespace ScholarshipService.API.GraphQL.Queries;

public class ScholarshipQuery
{
    [GraphQLDescription("Get all scholarships (optionally filtered by employee ID).")]
    public async Task<IEnumerable<ScholarshipMainDto>> GetScholarships(
        [Service] IMediator mediator,
        int? employeeId = null,
        int page = 1,
        int pageSize = 20)
    {
        var result = await mediator.Send(new GetScholarshipsQuery(employeeId, page, pageSize));
        return result.Items;
    }

    [GraphQLDescription("Get a single scholarship by ID.")]
    public async Task<ScholarshipMainDto?> GetScholarship(
        [Service] IMediator mediator,
        int id)
    {
        return await mediator.Send(new GetScholarshipByIdQuery(id));
    }

    [GraphQLDescription("Get all scholarship amount configurations.")]
    public async Task<IEnumerable<ScholarshipAmountDto>> GetScholarshipAmounts([Service] IMediator mediator)
    {
        return await mediator.Send(new GetScholarshipAmountsQuery());
    }
}
