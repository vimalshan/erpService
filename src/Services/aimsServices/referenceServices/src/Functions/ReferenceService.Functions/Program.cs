using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReferenceService.Infrastructure;
using ReferenceService.Infrastructure.Persistence;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=REFERENCEDB;Integrated Security=True;";
        
        // Add Infrastructure services
        services.AddInfrastructureServices(connectionString, context.Configuration);
        
        // Add Logging
        services.AddLogging();
    })
    .Build();

host.Run();
