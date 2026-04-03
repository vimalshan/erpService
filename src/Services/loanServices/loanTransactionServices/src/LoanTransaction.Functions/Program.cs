using LoanTransaction.Application;
using LoanTransaction.Functions;
using LoanTransaction.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.Configure<WorkerSettings>(
    builder.Configuration.GetSection("WorkerSettings"));

builder.Services.AddHostedService<LoanApplicationApprovedConsumer>();
builder.Services.AddHostedService<OverdueInstallmentScanner>();

var host = builder.Build();
host.Run();
