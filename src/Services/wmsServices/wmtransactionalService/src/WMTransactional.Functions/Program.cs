using WMTransactional.Application;
using WMTransactional.Infrastructure;
using WMTransactional.Functions.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<PendingOrderMonitorWorker>();
builder.Services.AddHostedService<OverdueShipmentWorker>();

var host = builder.Build();
host.Run();
