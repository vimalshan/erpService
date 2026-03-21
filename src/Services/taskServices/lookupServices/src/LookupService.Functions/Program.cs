using LookupService.Application;
using LookupService.Functions;
using LookupService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<LookupDataMonitorWorker>();
builder.Services.AddHostedService<AccessDetailCleanupWorker>();

var host = builder.Build();
host.Run();
