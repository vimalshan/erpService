using SupplierService.Application;
using SupplierService.Functions.Workers;
using SupplierService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<SupplierSyncWorker>();

var host = builder.Build();
host.Run();
