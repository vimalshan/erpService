using ReferenceDataService.Application;
using ReferenceDataService.Functions.Functions;
using ReferenceDataService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<DataCleanupFunction>();
builder.Services.AddHostedService<BlobProcessingFunction>();

var host = builder.Build();
host.Run();
