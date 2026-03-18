using HotChocolate.Execution.Configuration;
using AccidentManagementService.Application.Queries;
using AccidentManagementService.Domain.Entities;

namespace AccidentManagementService.GraphQL;

/// <summary>
/// GraphQL schema configuration using HotChocolate
/// Provides query and mutation types for accident management
/// </summary>
public static class GraphQLConfiguration
{
    /// <summary>
    /// Configures GraphQL server in dependency injection container
    /// Call this in Program.cs: services.ConfigureGraphQL()
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Configured service collection</returns>
    public static IServiceCollection ConfigureGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            // Query types
            .AddQueryType<AccidentQuery>()
            
            // Mutation types (if implemented)
            .AddMutationType<AccidentMutation>()
            
            // Subscription types (for real-time updates)
            .AddSubscriptionType<AccidentSubscription>()
            
            // Additional configuration
            .AddProjections()           // For filtering on relationships
            .AddFiltering()             // Global filtering support
            .AddSorting()               // Global sorting support
            .AddIntrospection()         // Enable schema introspection
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
            ;

        return services;
    }

    /// <summary>
    /// Maps GraphQL endpoints in the HTTP request pipeline
    /// Call this in Program.cs: app.MapGraphQLEndpoints()
    /// </summary>
    /// <param name="app">Web application builder</param>
    public static WebApplication MapGraphQLEndpoints(this WebApplication app)
    {
        // Main GraphQL endpoint
        app.MapGraphQL("/graphql");
        
        // GraphQL IDE (GraphiQL)
        app.MapGraphQLSchema("/graphql/schema.graphql");
        
        return app;
    }
}

/// <summary>
/// GraphQL Query type - maps to accidentQueries
/// Defines all queryable fields for accident data
/// </summary>
[GraphQLType("Query")]
public class AccidentQuery
{
    /// <summary>
    /// Gets a single accident report by database ID
    /// 
    /// Query:
    /// query {
    ///   accidentReportById(id: 123) {
    ///     id
    ///     accidentNumber
    ///     injuredPersonInfo { personName }
    ///   }
    /// }
    /// </summary>
    public async Task<AccidentReportGraphQLDto?> GetAccidentReportById(
        long id,
        [Service] IAccidentQueries queries,
        CancellationToken cancellationToken)
    {
        var result = await queries.GetAccidentReportById(id, cancellationToken);
        return result != null ? MapToGraphQL(result) : null;
    }

    /// <summary>
    /// Gets accident reports by company with pagination
    /// 
    /// Query:
    /// query {
    ///   accidentReportsByCompany(
    ///     companyCode: "COMP1"
    ///     pageNumber: 1
    ///     pageSize: 10
    ///   ) {
    ///     items { accidentNumber }
    ///     totalCount
    ///     totalPages
    ///   }
    /// }
    /// </summary>
    [GraphQLName("accidentReportsByCompany")]
    public async Task<AccidentReportPagedResultGraphQLDto> GetAccidentReportsByCompany(
        string companyCode,
        int pageNumber = 1,
        int pageSize = 10,
        [Service] IAccidentQueries queries,
        CancellationToken cancellationToken)
    {
        var query = new GetAccidentReportsByCompanyQuery(companyCode, pageNumber, pageSize);
        // Execute query via mediator and map results
        return new AccidentReportPagedResultGraphQLDto();
    }

    /// <summary>
    /// Gets accident statistics (aggregated data)
    /// 
    /// Query:
    /// query {
    ///   accidentStatistics {
    ///     totalAccidents
    ///     byStatus { statusId count }
    ///   }
    /// }
    /// </summary>
    public async Task<AccidentStatisticsGraphQLDto> GetAccidentStatistics(
        [Service] IAccidentQueries queries,
        CancellationToken cancellationToken)
    {
        // Execute statistics query and map results
        return new AccidentStatisticsGraphQLDto();
    }

    /// <summary>
    /// Gets all injury categories (master data)
    /// 
    /// Query:
    /// query {
    ///   injuryCategories { id name description }
    /// }
    /// </summary>
    [GraphQLName("injuryCategories")]
    public async Task<IEnumerable<InjuryCategoryGraphQLDto>> GetInjuryCategories(
        [Service] IAccidentQueries queries,
        CancellationToken cancellationToken)
    {
        // Fetch and map master data
        return new List<InjuryCategoryGraphQLDto>();
    }

    /// <summary>
    /// Gets all injury natures (master data)
    /// </summary>
    [GraphQLName("injuryNatures")]
    public async Task<IEnumerable<InjuryNatureGraphQLDto>> GetInjuryNatures(
        [Service] IAccidentQueries queries,
        CancellationToken cancellationToken)
    {
        return new List<InjuryNatureGraphQLDto>();
    }
}

/// <summary>
/// GraphQL Mutation type for modifying accident data
/// Defines all mutable operations
/// </summary>
[GraphQLType("Mutation")]
public class AccidentMutation
{
    /// <summary>
    /// Creates a new accident report
    /// 
    /// Mutation:
    /// mutation {
    ///   createAccidentReport(input: {
    ///     companyCode: "COMP1"
    ///     injuredPersonName: "John Doe"
    ///   }) {
    ///     accidentNumber
    ///     statusMessage
    ///   }
    /// }
    /// </summary>
    public async Task<CreateAccidentReportPayloadGraphQLDto> CreateAccidentReport(
        CreateAccidentReportInputGraphQLDto input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        // Convert GraphQL input to command, execute, and return payload
        return new CreateAccidentReportPayloadGraphQLDto();
    }

    /// <summary>
    /// Updates accident report status
    /// </summary>
    public async Task<UpdateAccidentStatusPayloadGraphQLDto> UpdateAccidentStatus(
        long accidentReportId,
        int newStatusId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return new UpdateAccidentStatusPayloadGraphQLDto();
    }

    /// <summary>
    /// Updates accident report severity level
    /// </summary>
    public async Task<UpdateAccidentSeverityPayloadGraphQLDto> UpdateAccidentSeverity(
        long accidentReportId,
        int newSeverityId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return new UpdateAccidentSeverityPayloadGraphQLDto();
    }
}

/// <summary>
/// GraphQL Subscription type for real-time updates via WebSocket
/// Demonstrates how to push updates to connected clients
/// </summary>
[GraphQLType("Subscription")]
public class AccidentSubscription
{
    /// <summary>
    /// Subscribes to new accident reports being created
    /// 
    /// Subscription:
    /// subscription {
    ///   onAccidentReportCreated {
    ///     accidentNumber
    ///     createdDate
    ///   }
    /// }
    /// </summary>
    [Subscribe]
    [GraphQLName("onAccidentReportCreated")]
    public IAsyncEnumerable<AccidentReportGraphQLDto> OnAccidentReportCreated(
        [Service] IIntegrationEventPublisher eventPublisher,
        CancellationToken cancellationToken)
    {
        // Subscribe to domain events and yield to clients
        return AsyncEnumerable.Empty<AccidentReportGraphQLDto>();
    }

    /// <summary>
    /// Subscribes to accident status changes
    /// </summary>
    [Subscribe]
    public IAsyncEnumerable<AccidentStatusChangedGraphQLDto> OnAccidentStatusChanged(
        CancellationToken cancellationToken)
    {
        return AsyncEnumerable.Empty<AccidentStatusChangedGraphQLDto>();
    }
}

#region GraphQL DTO Types

/// <summary>
/// GraphQL representation of AccidentReport
/// Automatically serialized to JSON by HotChocolate
/// </summary>
public class AccidentReportGraphQLDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string? AccidentNumber { get; set; }
    public string? CompanyCode { get; set; }
    
    [GraphQLName("employeeInfo")]
    public EmployeeInfoGraphQLDto? EmployeeInfo { get; set; }
    
    [GraphQLName("contractorInfo")]
    public ContractorInfoGraphQLDto? ContractorInfo { get; set; }
    
    [GraphQLName("injuredPersonInfo")]
    public InjuredPersonInfoGraphQLDto? InjuredPersonInfo { get; set; }
    
    [GraphQLName("accidentDetails")]
    public AccidentDetailsGraphQLDto? AccidentDetails { get; set; }
    
    [GraphQLName("injuryDetails")]
    public InjuryDetailsGraphQLDto? InjuryDetails { get; set; }
    
    [GraphQLName("treatmentInfo")]
    public TreatmentInfoGraphQLDto? TreatmentInfo { get; set; }
    
    public int? SeverityId { get; set; }
    public int? StatusId { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}

public class AccidentReportPagedResultGraphQLDto
{
    [GraphQLName("items")]
    public List<AccidentReportGraphQLDto> Items { get; set; } = new();
    
    [GraphQLName("pageNumber")]
    public int PageNumber { get; set; }
    
    [GraphQLName("pageSize")]
    public int PageSize { get; set; }
    
    [GraphQLName("totalCount")]
    public int TotalCount { get; set; }
    
    [GraphQLName("totalPages")]
    public int TotalPages { get; set; }
}

public class AccidentStatisticsGraphQLDto
{
    [GraphQLName("totalAccidents")]
    public int TotalAccidents { get; set; }
    
    [GraphQLName("bySeverity")]
    public List<CountByIdGraphQLDto> BySeverity { get; set; } = new();
    
    [GraphQLName("byStatus")]
    public List<CountByIdGraphQLDto> ByStatus { get; set; } = new();
}

public class CountByIdGraphQLDto
{
    [GraphQLName("id")]
    public int Id { get; set; }
    
    [GraphQLName("count")]
    public int Count { get; set; }
}

public class InjuryCategoryGraphQLDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class InjuryNatureGraphQLDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class EmployeeInfoGraphQLDto
{
    [GraphQLName("employeeNumber")]
    public string? EmployeeNumber { get; set; }
    
    [GraphQLName("employeeName")]
    public string? EmployeeName { get; set; }
    
    [GraphQLName("department")]
    public string? Department { get; set; }
}

public class ContractorInfoGraphQLDto
{
    [GraphQLName("contractorName")]
    public string? ContractorName { get; set; }
    
    [GraphQLName("contractorId")]
    public long? ContractorId { get; set; }
}

public class InjuredPersonInfoGraphQLDto
{
    [GraphQLName("personName")]
    public string? PersonName { get; set; }
    
    [GraphQLName("serialNumber")]
    public string? SerialNumber { get; set; }
    
    [GraphQLName("employeeStatus")]
    public string? EmployeeStatus { get; set; }
}

public class AccidentDetailsGraphQLDto
{
    [GraphQLName("location")]
    public string? Location { get; set; }
    
    [GraphQLName("accidentDateTime")]
    public DateTime? AccidentDateTime { get; set; }
    
    [GraphQLName("cause")]
    public string? Cause { get; set; }
    
    [GraphQLName("preventiveMeasures")]
    public string? PreventiveMeasures { get; set; }
}

public class InjuryDetailsGraphQLDto
{
    [GraphQLName("bodyPart")]
    public string? BodyPart { get; set; }
    
    [GraphQLName("category")]
    public InjuryCategoryGraphQLDto? Category { get; set; }
    
    [GraphQLName("nature")]
    public InjuryNatureGraphQLDto? Nature { get; set; }
}

public class TreatmentInfoGraphQLDto
{
    [GraphQLName("treatmentCentreName")]
    public string? TreatmentCentreName { get; set; }
    
    [GraphQLName("dateReceived")]
    public DateTime? DateReceived { get; set; }
    
    [GraphQLName("treatmentGiven")]
    public string? TreatmentGiven { get; set; }
}

public class CreateAccidentReportInputGraphQLDto
{
    [GraphQLName("companyCode")]
    public string? CompanyCode { get; set; }
    
    [GraphQLName("employeeNumber")]
    public string? EmployeeNumber { get; set; }
    
    [GraphQLName("injuredPersonName")]
    public string? InjuredPersonName { get; set; }
    
    [GraphQLName("location")]
    public string? Location { get; set; }
    
    [GraphQLName("accidentDateTime")]
    public DateTime? AccidentDateTime { get; set; }
}

public class CreateAccidentReportPayloadGraphQLDto
{
    [GraphQLName("accidentNumber")]
    public string? AccidentNumber { get; set; }
    
    [GraphQLName("statusMessage")]
    public string? StatusMessage { get; set; }
    
    [GraphQLName("success")]
    public bool Success { get; set; }
}

public class UpdateAccidentStatusPayloadGraphQLDto
{
    [GraphQLName("success")]
    public bool Success { get; set; }
    
    [GraphQLName("message")]
    public string? Message { get; set; }
}

public class UpdateAccidentSeverityPayloadGraphQLDto
{
    [GraphQLName("success")]
    public bool Success { get; set; }
    
    [GraphQLName("message")]
    public string? Message { get; set; }
}

public class AccidentStatusChangedGraphQLDto
{
    [GraphQLName("accidentNumber")]
    public string? AccidentNumber { get; set; }
    
    [GraphQLName("oldStatus")]
    public string? OldStatus { get; set; }
    
    [GraphQLName("newStatus")]
    public string? NewStatus { get; set; }
    
    [GraphQLName("changedAt")]
    public DateTime ChangedAt { get; set; }
}

#endregion

// Helper interfaces for DI
public interface IAccidentQueries
{
    Task<object?> GetAccidentReportById(long id, CancellationToken cancellationToken);
}

public interface IIntegrationEventPublisher
{
}

// Helper mapping method (implement based on your DTOs)
private static AccidentReportGraphQLDto MapToGraphQL(object result)
{
    // Implement mapping from domain/app DTOs to GraphQL DTOs
    return new AccidentReportGraphQLDto();
}
