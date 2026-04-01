using MasterService.API.GraphQL;
using MasterService.API.HealthChecks;
using MasterService.API.Middleware;
using MasterService.API.MinimalApis;
using MasterService.Application;
using MasterService.Infrastructure;
using MasterService.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Native .NET 10 OpenAPI document generation (Scalar UI at /scalar)
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, ctx, ct) =>
    {
        doc.Info.Title = "Master Service API";
        doc.Info.Version = "v1";
        doc.Info.Description = "ERP Master Module microservice — Skills, Trainings, Jobs, Categories & more.";
        return Task.CompletedTask;
    });
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<MasterQuery>()
    .AddMutationType<MasterMutation>()
    .AddType<MasterService.API.GraphQL.Types.SkillType>()
    .AddType<MasterService.API.GraphQL.Types.TrainingProviderType>()
    .AddType<MasterService.API.GraphQL.Types.JobMasterType>()
    .AddType<MasterService.API.GraphQL.Types.CategoryType>()
    .BindRuntimeType<char, HotChocolate.Types.StringType>()
    .BindRuntimeType<char?, HotChocolate.Types.StringType>()
    .AddTypeConverter<char, string>(c => c.ToString())
    .AddTypeConverter<string, char>(s => s.Length > 0 ? s[0] : default)
    .AddTypeConverter<char?, string>(c => c?.ToString() ?? "")
    .AddAuthorization();

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", HealthStatus.Unhealthy, ["db", "ready"])
    .AddCheck<RabbitMqHealthCheck>("rabbitmq", HealthStatus.Degraded, ["messaging"]);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Native OpenAPI document endpoint: /openapi/v1.json
    app.MapOpenApi();
    // Scalar API UI at /scalar/v1 (equivalent to Swagger UI)
    app.MapScalarApiReference(options =>
    {
        options.Title = "Master Service API";
        options.Theme = ScalarTheme.BluePlanet;
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapSkillEndpoints();
app.MapTrainingEndpoints();

app.MapGraphQL("/graphql");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = hc => hc.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await DbSeeder.SeedAsync(app.Services);

app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "MasterService host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }

