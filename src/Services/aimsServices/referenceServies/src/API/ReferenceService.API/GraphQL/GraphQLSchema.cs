using HotChocolate;
using HotChocolate.Execution.Configuration;
using MediatR;
using ReferenceService.Application.DTOs;
using ReferenceService.Application.Queries.LovType;
using ReferenceService.Application.Commands.LovType;

namespace ReferenceService.API.GraphQL;

/// <summary>
/// GraphQL Query type.
/// </summary>
public class Query
{
    public async Task<List<LovTypeDto>> GetLovTypes([Service] IMediator mediator)
    {
        var result = await mediator.Send(new GetAllLovTypesQuery(1, 100));
        return result.Items;
    }
    
    public async Task<LovTypeDto?> GetLovType(int id, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetLovTypeByIdQuery(id));
    }
}

/// <summary>
/// GraphQL Mutation type.
/// </summary>
public class Mutation
{
    public async Task<LovTypeDto?> CreateLovType(
        string typeName,
        string? description,
        int sequence,
        long modifiedBy,
        [Service] IMediator mediator)
    {
        var result = await mediator.Send(new CreateLovTypeCommand(typeName, description, sequence, modifiedBy));
        if (!result.Success) return null;
        return await mediator.Send(new GetLovTypeByIdQuery(result.Id));
    }
}

/// <summary>
/// GraphQL extension configuration.
/// </summary>
public static class GraphQLConfigurationExtensions
{
    public static void AddGraphQLConfiguration(this IRequestExecutorBuilder builder)
    {
        builder
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();
    }
}
