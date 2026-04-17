using WebsiteContentService.Application.Behaviors;
using WebsiteContentService.Application.Commands.Pages;
using WebsiteContentService.Application.Mappings;
using WebsiteContentService.Infrastructure;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebsiteContentService.AzureFunctions;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration(config =>
            {
                config.AddEnvironmentVariables();
            })
            .ConfigureServices(services =>
            {
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                    ?? throw new InvalidOperationException("Environment variable 'ConnectionStrings__DefaultConnection' not configured");

                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddEnvironmentVariables();
                var configuration = configBuilder.Build();

                services.AddInfrastructure(connectionString);
                services.AddExternalServices(configuration);

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,
                    typeof(CreateWebsitePageCommand).Assembly));
                services.AddAutoMapper(cfg => cfg.AddProfile<WebsiteContentMappingProfile>());
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                services.AddLogging();
            })
            .Build();

        host.Run();
    }
}
