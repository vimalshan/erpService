using System.Text;
using FinanceService.Application;
using FinanceService.Infrastructure;
using FinanceService.Infrastructure.Persistence;
using FinanceService.Infrastructure.Persistence.Seed;
using FinanceService.API.GraphQL.Queries;
using FinanceService.API.GraphQL.Mutations;
using FinanceService.API.GraphQL.Types;
using FinanceService.API.HealthChecks;
using FinanceService.API.Middleware;
using FinanceService.API.MinimalApis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Allow background services (RabbitMQ consumers) to fail without stopping the host
builder.Services.Configure<HostOptions>(options =>
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization();

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<FinanceQuery>()
    .AddMutationType<FinanceMutation>()
    .AddType<InvoiceType>()
    .AddType<InvoiceLineType>()
    .AddType<BatchType>()
    .AddType<BatchSubType>()
    .AddType<PaymentType>()
    .AddType<PaymentTermType>()
    .AddType<JvPostingDetailType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddSqlServer(
        builder.Configuration.GetConnectionString("FinanceDb")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" });

// Polly Circuit Breaker for HttpClient
var pollyConfig = builder.Configuration.GetSection("Polly");
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            int.Parse(pollyConfig["RetryCount"] ?? "3"),
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            int.Parse(pollyConfig["CircuitBreakerThreshold"] ?? "5"),
            TimeSpan.FromSeconds(int.Parse(pollyConfig["CircuitBreakerDurationSeconds"] ?? "30"))));

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapGraphQL();
app.MapFinanceEndpoints();
app.MapHealthChecks("/health");

// Database migration and seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FinanceDbContext>();
        await context.Database.MigrateAsync();
        var logger = services.GetRequiredService<ILogger<FinanceDbContext>>();
        await FinanceDbContextSeed.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database");
    }
}

app.Run();

public partial class Program { }

