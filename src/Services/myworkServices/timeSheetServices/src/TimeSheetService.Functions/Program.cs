using TimeSheetService.Application;
using TimeSheetService.Functions.Functions;
using TimeSheetService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<TimesheetNotificationFunction>();
builder.Services.AddHostedService<TimesheetReportFunction>();

var host = builder.Build();
host.Run();
