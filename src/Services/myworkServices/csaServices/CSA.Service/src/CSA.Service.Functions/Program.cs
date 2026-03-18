using CSA.Service.Application;
using CSA.Service.Functions;
using CSA.Service.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<SurveyReminderWorker>();
builder.Services.AddHostedService<EvidenceCleanupWorker>();

var host = builder.Build();
host.Run();
