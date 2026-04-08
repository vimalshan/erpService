using PFTransactionalService.Application;
using PFTransactionalService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<PFTransactionalService.Functions.Workers.MonthlyContributionWorker>();
builder.Services.AddHostedService<PFTransactionalService.Functions.Workers.InterestCalculationWorker>();

var host = builder.Build();
host.Run();
