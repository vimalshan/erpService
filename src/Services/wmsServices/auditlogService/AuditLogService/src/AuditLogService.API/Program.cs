using System.Text;
using AuditLogService.API.GraphQL;
using AuditLogService.API.Middleware;
using AuditLogService.Application;
using AuditLogService.Infrastructure;
using AuditLogService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuditLog API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<AuditLogQuery>()
    .AddMutationType<AuditLogMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" });

var app = builder.Build();

// Global exception handler middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuditLog API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// REST Controllers
app.MapControllers();

// GraphQL endpoint
app.MapGraphQL();

// Health checks
app.MapHealthChecks("/health");

// Minimal API endpoints
app.MapGet("/api/minimal/auditlogs/count", async (AuditLogService.Domain.Repositories.IAuditLogRepository repo) =>
{
    var count = await repo.GetCountAsync();
    return Results.Ok(new { Count = count });
}).WithTags("MinimalApi");

app.MapGet("/api/minimal/auditlogs/recent/{count:int}", async (int count, AuditLogService.Infrastructure.Persistence.Dapper.AuditLogDapperRepository dapperRepo) =>
{
    var logs = await dapperRepo.GetRecentLogsAsync(count);
    return Results.Ok(logs);
}).WithTags("MinimalApi");

app.MapGet("/api/minimal/auditlogs/search", async (
    string? tableName,
    string? action,
    DateTime? fromDate,
    DateTime? toDate,
    AuditLogService.Infrastructure.Persistence.Dapper.AuditLogDapperRepository dapperRepo) =>
{
    var logs = await dapperRepo.SearchLogsAsync(tableName, action, fromDate, toDate);
    return Results.Ok(logs);
}).WithTags("MinimalApi");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditLogDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await context.Database.EnsureCreatedAsync();
    await AuditLogDbSeeder.SeedAsync(context, logger);
}

app.Run();

