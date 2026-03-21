using WarehouseStructure.Application;
using WarehouseStructure.Infrastructure;
using WarehouseStructure.Functions;
using WarehouseStructure.Functions.Functions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<WarehouseCleanupFunction>();
builder.Services.AddHostedService<WarehouseDataSyncFunction>();

var host = builder.Build();
host.Run();
