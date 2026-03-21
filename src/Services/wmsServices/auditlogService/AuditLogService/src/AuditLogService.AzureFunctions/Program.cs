using AuditLogService.Application;
using AuditLogService.AzureFunctions;
using AuditLogService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<AuditLogArchivalWorker>();

var host = builder.Build();
host.Run();
