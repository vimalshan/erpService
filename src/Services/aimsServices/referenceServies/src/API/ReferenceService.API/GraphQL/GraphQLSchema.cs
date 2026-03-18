using HotChocolate;
using HotChocolate.Execution.Configuration;
using ReferenceService.Application.DTOs;

namespace ReferenceService.API.GraphQL;

/// <summary>
/// GraphQL Query type.
/// </summary>
[QueryType]
public class Query
{
    public async Task<List<LovTypeDto>> GetLovTypes([Service] IHttpClientFactory httpClientFactory)
    {
        // Implementation would call the REST API or database directly
        return await Task.FromResult(new List<LovTypeDto>());
    }
    
    public async Task<LovTypeDto?> GetLovType(int id, [Service] IHttpClientFactory httpClientFactory)
    {
        return null;
    }
}

/// <summary>
/// GraphQL Mutation type.
/// </summary>
[MutationType]
public class Mutation
{
    public async Task<LovTypeDto?> CreateLovType(
        string typeName,
        string? description,
        int sequence,
        long modifiedBy,
        [Service] IHttpClientFactory httpClientFactory)
    {
        // Implementation would call application layer
        return null;
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
