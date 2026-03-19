using ApprovalGroup.Application;
using ApprovalGroup.Infrastructure;
using ApprovalGroup.Infrastructure.Persistence;
using ApprovalGroup.API.Extensions;
using ApprovalGroup.API.GraphQL;
using ApprovalGroup.API.Middleware;
using ApprovalGroup.API.MinimalApis;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Core Services ─────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Application & Infrastructure ──────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── Authentication / Authorization (JWT) ─────────────────────
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── Swagger ───────────────────────────────────────────────────
builder.Services.AddSwaggerWithJwt();

// ── GraphQL via HotChocolate ──────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ApprovalGroupQuery>()
    .AddMutationType<ApprovalGroupMutation>();

// ── Health Checks ─────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" })
    .AddRabbitMQ(
        name: "rabbitmq",
        tags: new[] { "messaging" });

// ── CORS ──────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Approval Group API v1"));
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

// ── Health Check Endpoints ────────────────────────────────────
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

// ── Minimal APIs ──────────────────────────────────────────────
app.MapMinimalApiEndpoints();

// ── Auto-migrate on startup (dev only) ───────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApprovalGroupDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
