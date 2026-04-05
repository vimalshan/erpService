using TransactionService.Application;
using TransactionService.Functions;
using TransactionService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHostedService<TransactionWorker>();

var host = builder.Build();
host.Run();
