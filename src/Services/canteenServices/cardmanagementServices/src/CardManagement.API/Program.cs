using CardManagement.API.Endpoints;
using CardManagement.API.Extensions;
using CardManagement.API.GraphQL.Mutations;
using CardManagement.API.GraphQL.Queries;
using CardManagement.API.GraphQL.Types;
using CardManagement.API.Middleware;
using CardManagement.API.Services;
using CardManagement.Application;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Infrastructure;
using CardManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ---- Services ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger / OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CardManagement API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token"
    });
    c.AddSecurityRequirement(new()
    {
        {
            new() { Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

// Application + Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Auth
builder.Services.AddJwtAuthentication(builder.Configuration);

// HttpContext for CurrentUserService
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// GraphQL with Banana Cake Pop
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CardQuery>()
    .AddMutationType<CardMutation>()
    .AddType<GuestCardType>()
    .AddType<CanteenCardMapType>()
    .AddType<CardSettlementType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
var hcBuilder = builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sql-server",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "db", "sql" });

if (builder.Configuration.GetValue<bool>("RabbitMQ:Enabled", true))
{
    hcBuilder.AddRabbitMQ(
        _ => new RabbitMQ.Client.ConnectionFactory
        {
            HostName = builder.Configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = builder.Configuration["RabbitMQ:Username"] ?? "guest",
            Password = builder.Configuration["RabbitMQ:Password"] ?? "guest"
        }.CreateConnectionAsync(),
        name: "rabbitmq",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "messaging" });
}

var app = builder.Build();

// Run migrations + seed on startup
if (app.Environment.IsDevelopment())
{
    await SeedData.InitialiseAsync(app.Services);
}

// ---- Middleware pipeline ----
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardManagement API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Minimal API endpoints
app.MapAuthEndpoints();
app.MapCardEndpoints();

// GraphQL - Banana Cake Pop at /graphql
app.MapGraphQL();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await ctx.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = h => h.Tags.Contains("db") });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

app.Run();
