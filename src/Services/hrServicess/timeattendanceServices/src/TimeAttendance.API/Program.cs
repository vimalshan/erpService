using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using TimeAttendance.API.GraphQL.Mutations;
using TimeAttendance.API.GraphQL.Queries;
using TimeAttendance.API.GraphQL.Types;
using TimeAttendance.API.Middleware;
using TimeAttendance.API.MinimalApis;
using TimeAttendance.Application;
using TimeAttendance.Infrastructure;
using TimeAttendance.Infrastructure.Messaging;
using TimeAttendance.Infrastructure.Persistence.Seed;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// Application & Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// MVC Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Time & Attendance API",
        Version = "v1",
        Description = "Microservice for Time Attendance management."
    });
    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token."
    });
    options.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            []
        }
    });
});

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<AbsenteeismDetailType>()
    .AddType<AbsenteeismMisType>()
    .AddAuthorization();

// Health Checks
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var rabbitOpts = builder.Configuration.GetSection(RabbitMqOptions.SectionName);

builder.Services.AddHealthChecks()
    .AddSqlServer(connStr, name: "database", tags: ["db", "sql"])
    .AddRabbitMQ(
        sp =>
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = rabbitOpts["Host"] ?? "localhost",
                Port = int.TryParse(rabbitOpts["Port"], out var port) ? port : 5672,
                UserName = rabbitOpts["UserName"] ?? "guest",
                Password = rabbitOpts["Password"] ?? "guest"
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        },
        name: "rabbitmq",
        tags: ["messaging", "rabbitmq"]);

// CORS
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Middleware pipeline
app.UseGlobalExceptionHandler();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Time Attendance API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapAbsenteeismEndpoints();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = WriteHealthCheckResponse,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});
app.MapHealthChecks("/health/db", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("db"),
    ResponseWriter = WriteHealthCheckResponse
});
app.MapHealthChecks("/health/messaging", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("messaging"),
    ResponseWriter = WriteHealthCheckResponse
});

if (app.Environment.IsDevelopment())
    await DbInitializer.InitializeAsync(app.Services);

await app.RunAsync();

}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    await Log.CloseAndFlushAsync();
}

static Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";
    var result = System.Text.Json.JsonSerializer.Serialize(new
    {
        status = report.Status.ToString(),
        checks = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            description = e.Value.Description,
            duration = e.Value.Duration.TotalMilliseconds
        })
    });
    return context.Response.WriteAsync(result);
}
