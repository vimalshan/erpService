using EmployeeService.API.GraphQL;

namespace EmployeeService.API.Extensions;

/// <summary>
/// Extension method for configuring GraphQL with HotChocolate
/// </summary>
public static class GraphQLExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<EmployeeQuery>()
            .AddMutationType<EmployeeMutation>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = !configuration.GetValue<bool>("IsProduction"))
            .AddDefaultTransactionScopeHandler();

        return services;
    }

}
