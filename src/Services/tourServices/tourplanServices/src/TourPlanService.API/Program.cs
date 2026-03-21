using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TourPlanService.API.Middleware;
using TourPlanService.API.MinimalApis;
using TourPlanService.Application;
using TourPlanService.Infrastructure;
using TourPlanService.Infrastructure.Data;
using TourPlanService.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────────────────────
// Application & Infrastructure layers
// ──────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ──────────────────────────────────────────────
// JWT Authentication
// ──────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Manager", "Admin"));
});

// ──────────────────────────────────────────────
// Controllers
// ──────────────────────────────────────────────
builder.Services.AddControllers();

// ──────────────────────────────────────────────
// Swagger / OpenAPI
// ──────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TourPlan Service API",
        Version = "v1",
        Description = "Tour Plan Management Microservice"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: \"Bearer {token}\"",
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

// ──────────────────────────────────────────────
// GraphQL via HotChocolate
// ──────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TourPlanService.API.GraphQL.TourPlanQuery>()
    .AddMutationType<TourPlanService.API.GraphQL.TourPlanMutation>()
    .AddType<TourPlanService.API.GraphQL.Types.TourPlanType>()
    .AddType<TourPlanService.API.GraphQL.Types.ForexRequisitionType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// ──────────────────────────────────────────────
// Health Checks
// ──────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("TourPlanDb")!,
        name: "sql-server",
        tags: ["db", "sql", "sqlserver"]);

// CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ──────────────────────────────────────────────
// Build App
// ──────────────────────────────────────────────
var app = builder.Build();

// Apply EF migrations and seed on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TourPlanDbContext>();
    await dbContext.Database.MigrateAsync();
    await TourPlanSeedData.SeedAsync(dbContext);
}

// Warm up GraphQL schema at startup to detect schema errors early
try
{
    var executorResolver = app.Services.GetRequiredService<HotChocolate.Execution.IRequestExecutorResolver>();
    await executorResolver.GetRequestExecutorAsync();
    app.Logger.LogInformation("GraphQL schema built successfully.");
}
catch (HotChocolate.SchemaException ex)
{
    static void LogSchemaErrors(ILogger logger, HotChocolate.SchemaException e, int depth = 0)
    {
        string indent = new string(' ', depth * 2);
        foreach (var err in e.Errors)
        {
            logger.LogError("{Indent}Schema error [{Code}]: {Message}", indent, err.Code ?? "null", err.Message);
            if (err.TypeSystemObject is not null)
                logger.LogError("{Indent}  TypeSystem: {Type}", indent, err.TypeSystemObject.GetType().Name);
            if (err.Exception is HotChocolate.SchemaException nested)
                LogSchemaErrors(logger, nested, depth + 1);
            else if (err.Exception is not null)
                logger.LogError("{Indent}  Exception: {Msg}", indent, err.Exception.Message);
        }
    }
    LogSchemaErrors(app.Logger, ex);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "GraphQL schema build failed: {Message}", ex.Message);
}

// ──────────────────────────────────────────────
// Middleware Pipeline
// ──────────────────────────────────────────────
app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TourPlan API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// REST Controllers
app.MapControllers();

// Minimal APIs
app.MapTourPlanEndpoints();

// GraphQL
app.MapGraphQL("/graphql");

// Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

app.Run();
