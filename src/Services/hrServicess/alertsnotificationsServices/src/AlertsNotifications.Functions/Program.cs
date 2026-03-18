using AlertsNotifications.Application;
using AlertsNotifications.Functions;
using AlertsNotifications.Functions.Workers;
using AlertsNotifications.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<ProbationAlertWorker>();
builder.Services.AddHostedService<CircularExpiryWorker>();

var host = builder.Build();
host.Run();
