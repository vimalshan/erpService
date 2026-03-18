using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Document.Functions.Functions;
using Document.Infrastructure;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure(context.Configuration);
        services.AddHostedService<LetterCleanupWorker>();
        services.AddHostedService<BlobProcessorWorker>();
    })
    .Build();

await host.RunAsync();
