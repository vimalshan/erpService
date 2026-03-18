using SettlementService.Application;
using SettlementService.Infrastructure;
using SettlementService.Functions.BackgroundTasks;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHostedService<SettlementExpiryWorker>();
builder.Services.AddHostedService<SettlementReportWorker>();

var host = builder.Build();
await host.RunAsync();
