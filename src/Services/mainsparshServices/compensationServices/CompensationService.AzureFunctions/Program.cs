using CompensationService.Infrastructure;
using CompensationService.Application.Mappings;
using CompensationService.Application.Behaviors;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CompensationService.AzureFunctions;

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
                // Infrastructure
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                    ?? "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"CompensationService.AzureFunctions\";Command Timeout=0";
                
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddEnvironmentVariables();
                var configuration = configBuilder.Build();
                
                services.AddInfrastructure(connectionString);
                services.AddExternalServices(configuration);

                // Application services
                services.AddMediatR(typeof(Program).Assembly);
                services.AddAutoMapper(typeof(CompensationGradeMappingProfile));
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                // Logging
                services.AddLogging();
            })
            .Build();

        host.Run();
    }
}
