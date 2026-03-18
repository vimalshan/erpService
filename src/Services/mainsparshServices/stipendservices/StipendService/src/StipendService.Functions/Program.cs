using StipendService.Application;
using StipendService.Functions;
using StipendService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHostedService<StipendMonthlyWorker>();

var host = builder.Build();
host.Run();
