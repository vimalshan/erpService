using WorkOrderService.Application;
using WorkOrderService.Functions.Functions;
using WorkOrderService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<WorkOrderCleanupFunction>();
builder.Services.AddHostedService<OverdueWorkOrderNotificationFunction>();

var host = builder.Build();
host.Run();
