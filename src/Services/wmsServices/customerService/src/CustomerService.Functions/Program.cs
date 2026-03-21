using CustomerService.Application;
using CustomerService.Functions;
using CustomerService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<CustomerCleanupWorker>();
builder.Services.AddHostedService<BlobCleanupWorker>();

var host = builder.Build();
host.Run();
