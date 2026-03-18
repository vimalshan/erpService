using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using ReportingService.Application.DTOs;
using ReportingService.Application.Queries;
using MediatR;

namespace ReportingService.API.GraphQL;

public class Query
{
    private readonly IMediator _mediator;

    public Query(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<AppraisalDto?> GetAppraisalAsync(long id)
    {
        return await _mediator.Send(new GetAppraisalByIdQuery { Id = id });
    }

    public async Task<IEnumerable<AppraisalDto>> GetApprisalsAsync()
    {
        return await _mediator.Send(new GetAllApprisalsQuery());
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor.Name("Query");
        descriptor
            .Field(q => q.GetAppraisalAsync(default))
            .Name("getAppraisal")
            .Type<AppraisalType>();
        descriptor
            .Field(q => q.GetApprisalsAsync())
            .Name("getAppraisals")
            .Type<NonNullType<ListType<AppraisalType>>>();
    }
}
