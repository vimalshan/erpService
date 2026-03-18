using ContributionService.Application;
using ContributionService.Functions.BackgroundTasks;
using ContributionService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<MonthlyContributionProcessor>();
builder.Services.AddHostedService<ContributionLogCleanupTask>();
builder.Services.AddHostedService<BlobStorageSyncTask>();

var host = builder.Build();
host.Run();
