using EnergyService.Application;
using EnergyService.Functions;
using EnergyService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<EnergyAggregationWorker>();
builder.Services.AddHostedService<ThresholdAlertWorker>();

var host = builder.Build();
host.Run();
