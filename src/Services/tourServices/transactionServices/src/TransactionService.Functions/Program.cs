using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionService.Application;
using TransactionService.Infrastructure.Persistence;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Register DbContext
var connectionString = builder.Configuration["ConnectionStrings:TransactionDb"]
    ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TOURDB;Integrated Security=True;TrustServerCertificate=True";

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(connectionString,
        sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

// Register Application layer
builder.Services.AddApplication();

builder.Build().Run();
