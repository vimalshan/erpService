using MenuAndSecurityService.Application;
using MenuAndSecurityService.Functions.Workers;
using MenuAndSecurityService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<MenuCleanupWorker>();
builder.Services.AddHostedService<AuditLogWorker>();

var host = builder.Build();
host.Run();
