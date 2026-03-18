using FillingOperationService.API.GraphQL;
using FillingOperationService.API.Middleware;
using FillingOperationService.API.MinimalApis;
using FillingOperationService.Application;
using FillingOperationService.Infrastructure;
using FillingOperationService.Infrastructure.Persistence;
using FillingOperationService.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.CircuitBreaker;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

// ─── Serilog bootstrap ──────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    // ─── Application & Infrastructure DI ────────────────────────────────────
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // ─── Controllers ────────────────────────────────────────────────────────
    builder.Services.AddControllers();

    // ─── JWT Authentication ──────────────────────────────────────────────────
    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key is not configured.");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddAuthorization();

    // ─── Native .NET OpenAPI (replaces Swashbuckle setup) ───────────────────
    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((doc, ctx, _) =>
        {
            doc.Info = new() { Title = "Filling Operation Service API", Version = "v1" };
            return Task.CompletedTask;
        });
    });

    // ─── GraphQL (HotChocolate) ──────────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<FillingOperationsQuery>()
        .AddMutationType<FillingOperationsMutation>();

    // ─── Health Checks ───────────────────────────────────────────────────────
    builder.Services.AddHealthChecks()
        .AddSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")!,
            name: "sqlserver",
            tags: ["db", "sql"]);

    // ─── Polly Circuit Breaker ────────────────────────────────────────────────
    builder.Services.AddResiliencePipeline("filling-ops", pipelineBuilder =>
    {
        pipelineBuilder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            FailureRatio = 0.5,
            SamplingDuration = TimeSpan.FromSeconds(30),
            MinimumThroughput = 10,
            BreakDuration = TimeSpan.FromSeconds(60),
            OnOpened = args =>
            {
                Log.Warning("Circuit breaker opened for filling-ops.");
                return ValueTask.CompletedTask;
            }
        });
    });

    // ─── Build ───────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ─── Migrate & Seed ──────────────────────────────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FillingOperationsDbContext>();
        await db.Database.MigrateAsync();
    }
    await FillingOperationsSeedData.SeedAsync(app.Services);

    // ─── Middleware pipeline ─────────────────────────────────────────────────
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        // Scalar UI accessible at /scalar/v1
        app.MapScalarApiReference("/scalar", options =>
        {
            options.WithTitle("Filling Operation Service API");
        });
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapGraphQL("/graphql");
    app.MapHealthChecks("/health");
    app.MapFillingOperationsEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}


