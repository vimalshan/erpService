using TaskTransactional.Application;
using TaskTransactional.Functions;
using TaskTransactional.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<ComplaintMonitorWorker>();
builder.Services.AddHostedService<EscalationCheckWorker>();

var host = builder.Build();
host.Run();
