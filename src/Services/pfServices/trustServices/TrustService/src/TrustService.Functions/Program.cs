using MassTransit;
using Microsoft.EntityFrameworkCore;
using TrustService.Application;
using TrustService.Infrastructure;
using TrustService.Infrastructure.Messaging.Consumers;
using TrustService.Functions.Workers;

var builder = Host.CreateApplicationBuilder(args);

// Application & Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Background workers
builder.Services.AddHostedService<TrustAuditCleanupWorker>();
builder.Services.AddHostedService<TrustExpirationCheckerWorker>();

var host = builder.Build();
host.Run();
