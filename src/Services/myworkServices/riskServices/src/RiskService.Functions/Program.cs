using RiskService.Application;
using RiskService.Functions.Functions;
using RiskService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Background tasks
builder.Services.AddHostedService<MitigationDueDateChecker>();
builder.Services.AddHostedService<SelfAssessmentReminderService>();
builder.Services.AddHostedService<BlobCleanupService>();

var host = builder.Build();
host.Run();
