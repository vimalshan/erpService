using LocationService.Application.DTOs;
using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;

namespace LocationService.API.GraphQL
{
    /// <summary>
    /// GraphQL Type definitions for Location, Room, and Resource
    /// </summary>
    public class LocationType
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string LocationStatus { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class RoomType
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int? RoomCapacity { get; set; }
        public string? RoomTypeValue { get; set; }
        public string RoomStatus { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }

    public class RoomResourceType
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public string ResourceCode { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public int? ResourceQuantity { get; set; }
        public string ResourceStatus { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// GraphQL Query type
    /// </summary>
    public class Query
    {
        public string Hello => "Hello from GraphQL!";
    }

    /// <summary>
    /// GraphQL Mutation type
    /// </summary>
    public class Mutation
    {
        public string Message => "Mutations are ready";
    }

    /// <summary>
    /// GraphQL schema configuration extension
    /// </summary>
    public static class GraphQLSchemaConfiguration
    {
        public static IRequestExecutorBuilder AddLocationServiceSchema(this IRequestExecutorBuilder builder)
        {
            return builder
                .AddQueryType<Query>()
                .AddMutationType<Mutation>();
        }
    }
}
