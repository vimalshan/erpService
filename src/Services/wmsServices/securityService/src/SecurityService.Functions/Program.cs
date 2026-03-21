using SecurityService.Application;
using SecurityService.Functions.BackgroundTasks;
using SecurityService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Background task workers
builder.Services.AddHostedService<UserCleanupWorker>();
builder.Services.AddHostedService<AuditLogWorker>();

var host = builder.Build();
host.Run();
