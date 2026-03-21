using TourPlanService.Application;
using TourPlanService.Functions;
using TourPlanService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<TourPlanReminderWorker>();
builder.Services.AddHostedService<TourPlanExpiryWorker>();

var host = builder.Build();
host.Run();
