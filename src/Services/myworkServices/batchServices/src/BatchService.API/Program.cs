using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using BatchService.API.Auth;
using BatchService.API.HealthChecks;
using BatchService.API.Middleware;
using BatchService.API.MinimalApis;
using BatchService.API.OpenApi;
using BatchService.Application;
using BatchService.Infrastructure;
using BatchService.Infrastructure.Persistence;
using BatchService.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<BatchService.API.GraphQL.BatchQuery>()
    .AddMutationType<BatchService.API.GraphQL.BatchMutation>()
    .AddAuthorization();

var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("Jwt:Key not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"]   ?? "BatchService",
            ValidAudience            = builder.Configuration["Jwt:Audience"] ?? "BatchServiceClients",
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddHealthChecks()
    .AddCheck<BatchDatabaseHealthCheck>("database", HealthStatus.Unhealthy, ["db"]);

// Resilience pipeline with built-in circuit breaker + retry via Microsoft.Extensions.Http.Resilience
builder.Services.AddHttpClient("ExternalApi", c =>
    c.BaseAddress = new Uri(builder.Configuration["ExternalApi:BaseUrl"] ?? "https://localhost"))
    .AddStandardResilienceHandler();

var app = builder.Build();

// Auto-migrate EF + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BatchDbContext>();
    await db.Database.MigrateAsync();
    await BatchDataSeeder.SeedAsync(db);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "BatchService v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapBatchEndpoints();
app.MapAuthEndpoints();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name        = e.Key,
                status      = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await ctx.Response.WriteAsync(result);
    }
});

app.Run();

