using EmployeeService.Application;
using EmployeeService.Infrastructure;
using EmployeeService.API.Extensions;
using EmployeeService.API.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .MinimumLevel.Information()
        .WriteTo.Console()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", "EmployeeService"));

// Add services to the container
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

// Add JWT Token Generator
builder.Services.AddScoped<EmployeeService.API.Utilities.JwtTokenGenerator>();

// Add GraphQL
builder.Services.AddGraphQLServices(builder.Configuration);

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

// Add Controllers
builder.Services.AddControllers();

// Add OpenAPI/Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Health Checks
builder.Services.AddHealthChecks();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Migrate database
using (var scope = app.Services.CreateAsyncScope())
{
    await app.Services.MigrateAndSeedAsync();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

// Add custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();

// Liveness: excludes external-dependency checks (RabbitMQ/MassTransit)
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => !check.Tags.Contains("masstransit"),
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), error = e.Value.Exception?.Message })
        });
        await ctx.Response.WriteAsync(result);
    }
});

// Readiness: all checks including RabbitMQ/MassTransit
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), error = e.Value.Exception?.Message })
        });
        await ctx.Response.WriteAsync(result);
    }
});

// Map minimal APIs for example
app.MapEmployeeEndpoints();

// Map GraphQL endpoint
app.MapGraphQL();

app.Run();
