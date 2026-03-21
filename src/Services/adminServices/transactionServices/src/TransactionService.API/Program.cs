using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TransactionService.API.Authentication;
using TransactionService.API.GraphQL;
using TransactionService.API.Middleware;
using TransactionService.API.MinimalApis;
using TransactionService.Application;
using TransactionService.Infrastructure;
using TransactionService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ── Layer Registration ──
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

// ── Authentication & Authorization (JWT) ──
builder.Services.AddJwtAuthentication(configuration);

// ── Controllers ──
builder.Services.AddControllers();

// ── API Versioning ──
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ── Swagger / OpenAPI ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transaction Service API",
        Version = "v1",
        Description = "Stationery request, order, and budget transaction management."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    options.AddSecurityRequirement(doc =>
    {
        var requirement = new OpenApiSecurityRequirement();
        var schemeRef = new OpenApiSecuritySchemeReference("Bearer", doc);
        requirement.Add(schemeRef, []);
        return requirement;
    });
});

// ── GraphQL (HotChocolate) ──
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

// ── Health Checks ──
var connectionString = configuration.GetConnectionString("TransactionDb")!;
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "SQLServer", tags: ["db", "sql"]);

// ── CORS ──
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:3000"];
        policy.WithOrigins(origins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ── Middleware Pipeline ──
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Service API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ── Endpoints ──
app.MapControllers();
app.MapTransactionMinimalApis();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/db", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

// ── Database Migration on Startup ──
if (configuration.GetValue<bool>("Database:AutoMigrate"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();
