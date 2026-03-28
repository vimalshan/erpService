using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using ReportingService.Application.DTOs;
using ReportingService.Application.Queries;
using MediatR;

namespace ReportingService.API.GraphQL;

public class Query
{
    public async Task<AppraisalDto?> GetAppraisalAsync([Service] IMediator mediator, long id)
    {
        return await mediator.Send(new GetAppraisalByIdQuery { Id = id });
    }

    public async Task<IEnumerable<AppraisalDto>> GetApprisalsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllApprisalsQuery());
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor.Name("Query");
        descriptor
            .Field(q => q.GetAppraisalAsync(default!, default))
            .Name("getAppraisal")
            .Type<AppraisalType>();
        descriptor
            .Field(q => q.GetApprisalsAsync(default!))
            .Name("getAppraisals")
            .Type<NonNullType<ListType<AppraisalType>>>();
    }
}
