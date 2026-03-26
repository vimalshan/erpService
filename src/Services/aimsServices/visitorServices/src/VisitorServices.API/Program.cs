using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VisitorServices.Application;
using VisitorServices.Infrastructure;
using VisitorServices.API.Middleware;
using VisitorServices.API.GraphQL;
using VisitorServices.API.MinimalApis;
using VisitorServices.API.HealthChecks;
using VisitorServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure layers ─────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─── MVC Controllers ──────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Visitor Services API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
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

// ─── JWT Authentication ───────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// ─── GraphQL (HotChocolate) ───────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// ─── Health Checks ────────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", tags: ["db"])
    .AddSqlServer(
        builder.Configuration.GetConnectionString("VisitorDb")!,
        name: "sqlserver",
        tags: ["db"]);

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ─── Middleware pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ─── Swagger ─────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Visitor Services v1");
        c.RoutePrefix = "swagger";
    });
}

// ─── REST Controllers ─────────────────────────────────────────────────────────
app.MapControllers();

// ─── Minimal API endpoints ────────────────────────────────────────────────────
app.MapVisitorEndpoints();
app.MapApprovalEndpoints();

// ─── GraphQL endpoint ─────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ─── Health Checks ────────────────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/db", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ─── Auto-migrate and seed on startup ────────────────────────────────────────
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<VisitorDbContext>();
    var seedLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await db.Database.MigrateAsync();
        seedLogger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        seedLogger.LogError(ex, "Database migration failed. Service will continue without database.");
    }

    if (app.Environment.IsDevelopment())
    {
        await DbInitializer.SeedAsync(db, seedLogger);
    }
}

await app.RunAsync();
