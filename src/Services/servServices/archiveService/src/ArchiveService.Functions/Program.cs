using ArchiveService.Application;
using ArchiveService.Functions;
using ArchiveService.Functions.Workers;
using ArchiveService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<ArchiveCleanupWorker>();
builder.Services.AddHostedService<BlobSyncWorker>();

var host = builder.Build();
host.Run();
