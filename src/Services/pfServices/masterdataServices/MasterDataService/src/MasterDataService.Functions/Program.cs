using MasterDataService.Application;
using MasterDataService.Functions.BackgroundServices;
using MasterDataService.Functions.Consumers;
using MasterDataService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Background workers
builder.Services.AddHostedService<LovSyncWorker>();
builder.Services.AddHostedService<ConfigurationAuditWorker>();
builder.Services.AddHostedService<RateExpiryCheckerWorker>();

// RabbitMQ Consumers
builder.Services.AddHostedService<LovChangeConsumer>();
builder.Services.AddHostedService<ConfigAuditConsumer>();

var host = builder.Build();
host.Run();

