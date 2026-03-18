namespace CheckupManagementService.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using CheckupManagementService.Infrastructure.Mapping;

/// <summary>
/// Dependency injection extension for AutoMapper configuration
/// </summary>
public static class AutoMapperServiceExtension
{
    /// <summary>
    /// Add AutoMapper services to the DI container
    /// </summary>
    public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CheckupMappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}
