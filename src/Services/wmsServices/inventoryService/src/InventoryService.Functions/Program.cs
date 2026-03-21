using InventoryService.Functions;
using InventoryService.Functions.Workers;
using InventoryService.Application;
using InventoryService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<LowStockMonitorWorker>();
builder.Services.AddHostedService<InventorySnapshotWorker>();

var host = builder.Build();
host.Run();
