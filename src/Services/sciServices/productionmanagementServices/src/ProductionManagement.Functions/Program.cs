using ProductionManagement.Application;
using ProductionManagement.Functions.Workers;
using ProductionManagement.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Register background workers (Azure Function-like tasks)
builder.Services.AddHostedService<ProductionPlanExpiryWorker>();
builder.Services.AddHostedService<ProductionReportGeneratorWorker>();
builder.Services.AddHostedService<NormsCleanupWorker>();

var host = builder.Build();
host.Run();
