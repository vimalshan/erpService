using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionProcessing.Application;
using TransactionProcessing.Infrastructure;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var configuration = builder.Configuration;
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplicationInsightsTelemetryWorkerService();

builder.Build().Run();
