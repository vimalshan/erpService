using EmployeePrideManagement.Functions.Functions;
using EmployeePrideManagement.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHostedService<PrideMomentArchiveWorker>();
builder.Services.AddHostedService<PrideMomentImageCleanupWorker>();

var host = builder.Build();
host.Run();
