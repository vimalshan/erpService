using EmployeeService.API.GraphQL;

namespace EmployeeService.API.Extensions;

/// <summary>
/// Extension method for configuring GraphQL with HotChocolate
/// </summary>
public static class GraphQLExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<HotChocolate.Authorization.IAuthorizationHandler, AspNetCoreAuthorizationHandler>();

        services
            .AddGraphQLServer()
            .AddQueryType<EmployeeQuery>()
            .AddMutationType<EmployeeMutation>()
            .AddAuthorizationCore()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = !configuration.GetValue<bool>("IsProduction"))
            .AddDefaultTransactionScopeHandler();

        return services;
    }

}
