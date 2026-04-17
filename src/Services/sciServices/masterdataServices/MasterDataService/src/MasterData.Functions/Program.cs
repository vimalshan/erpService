using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MasterData.Domain.Aggregates;
using MasterData.Infrastructure;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        
        // Add infrastructure services
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? throw new InvalidOperationException("Environment variable 'ConnectionStrings__DefaultConnection' not configured");
        services.AddInfrastructureServices(connectionString);

        // Add Blob Storage client
        var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") 
            ?? "DefaultEndpointsProtocol=https;AccountName=youraccount;AccountKey=yourkey;EndpointSuffix=core.windows.net";
        services.AddScoped(_ => 
            new BlobContainerClient(
                new Uri($"https://youraccount.blob.core.windows.net/masterdata-images"),
                new Azure.Storage.StorageSharedKeyCredential("youraccount", "yourkey"))
        );
    })
    .Build();

host.Run();
