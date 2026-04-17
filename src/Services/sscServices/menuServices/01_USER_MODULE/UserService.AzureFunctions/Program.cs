using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Add Azure Blob Storage
        var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        services.AddSingleton(x => new BlobContainerClient(
            new Uri($"{storageConnectionString}/userprofiles"),
            new DefaultAzureCredential()));

        // Add Logging
        services.AddLogging();
    })
    .Build();

host.Run();
