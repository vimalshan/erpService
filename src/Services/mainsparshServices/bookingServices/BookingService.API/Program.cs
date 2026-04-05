using BookingService.Application;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Messaging;
using BookingService.Infrastructure.Persistence.Seed;
using BookingService.Infrastructure.Security;
using BookingService.API.GraphQL;
using BookingService.API.HealthChecks;
using BookingService.API.Middleware;
using BookingService.API.MinimalApis;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure Layers ──────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─── Controllers + Swagger ────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── GraphQL (HotChocolate) ───────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
    .AddTypeExtension<BookingQuery>()
    .AddMutationType(d => d.Name("Mutation"))
    .AddTypeExtension<BookingMutation>();

// ─── Health Checks ────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", failureStatus: HealthStatus.Unhealthy, tags: ["db"]);

// ─── CORS ─────────────────────────────────────────────────────────
builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ─── Middleware Pipeline ──────────────────────────────────────────
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingService API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapBookingEndpoints();

// ─── Health Check Endpoints ───────────────────────────────────────
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
app.MapHealthChecks("/health/db", new HealthCheckOptions { Predicate = r => r.Tags.Contains("db") });

// ─── Auth Token (Dev / Test only) ───────────────────────────────
app.MapPost("/api/auth/token", (TokenRequest req, IJwtService jwtService) =>
{
    var token = jwtService.GenerateToken(req.UserId, req.Email, req.Roles ?? ["User"]);
    return Results.Ok(new { token });
}).WithTags("Auth").AllowAnonymous();

// ─── RabbitMQ Test ────────────────────────────────────────────────
app.MapGet("/api/rabbitmq/test", async (IServiceProvider sp) =>
{
    try
    {
        var publisher = sp.GetRequiredService<IMessagePublisher>();
        await publisher.PublishAsync("booking.test", new { message = "test", timestamp = DateTime.UtcNow });
        return Results.Ok(new { status = "connected", message = "Test message published to booking.exchange" });
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "unavailable", message = ex.Message }, statusCode: 503);
    }
}).WithTags("RabbitMQ").AllowAnonymous();

// ─── Database Seed ────────────────────────────────────────────────
await BookingDbContextSeed.SeedAsync(app.Services);

app.Run();

record TokenRequest(long UserId, string Email, string[]? Roles);
