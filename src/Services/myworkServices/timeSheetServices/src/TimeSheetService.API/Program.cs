using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using TimeSheetService.API.Authentication;
using TimeSheetService.API.GraphQL.Mutations;
using TimeSheetService.API.GraphQL.Queries;
using TimeSheetService.API.HealthChecks;
using TimeSheetService.API.Middleware;
using TimeSheetService.API.MinimalApis;
using TimeSheetService.Application;
using TimeSheetService.Infrastructure;
using TimeSheetService.Infrastructure.Persistence;
using TimeSheetService.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure Layers ────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ─── JWT Authentication ─────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<TokenService>();

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
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});
builder.Services.AddAuthorization();

// ─── Controllers ────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── Swagger / OpenAPI ──────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── GraphQL (HotChocolate) ─────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TimesheetQueries>()
    .AddMutationType<TimesheetMutations>();

// ─── Health Checks ──────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddDbContextCheck<TimeSheetDbContext>("ef-database");

// ─── Polly Circuit Breaker for HttpClient ───────────────────────────────────
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            builder.Configuration.GetValue<int>("Polly:RetryCount"),
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            builder.Configuration.GetValue<int>("Polly:CircuitBreakerExceptionsAllowedBeforeBreaking"),
            TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("Polly:CircuitBreakerDurationOfBreakInSeconds"))));

// ─── MediatR for domain event handlers in API assembly ──────────────────────
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// ─── Middleware Pipeline ────────────────────────────────────────────────────
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeSheet Service V1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ─── Map Endpoints ──────────────────────────────────────────────────────────
app.MapControllers();
app.MapTimesheetEndpoints();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// ─── Database Migration & Seed ──────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TimeSheetDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Applying EF Core migrations...");
    await db.Database.MigrateAsync();
    logger.LogInformation("Migrations applied successfully.");

    await TimeSheetDbContextSeed.SeedAsync(db, logger);
}

app.Run();

