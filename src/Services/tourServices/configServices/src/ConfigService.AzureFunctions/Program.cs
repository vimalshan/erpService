using ConfigService.AzureFunctions;
using ConfigService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<BlobCleanupWorker>();

var host = builder.Build();
host.Run();
