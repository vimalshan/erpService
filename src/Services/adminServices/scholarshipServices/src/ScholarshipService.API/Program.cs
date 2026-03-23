using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScholarshipService.API.Endpoints.V1;
using ScholarshipService.API.GraphQL.Mutations;
using ScholarshipService.API.GraphQL.Queries;
using ScholarshipService.API.GraphQL.Subscriptions;
using ScholarshipService.API.GraphQL.Types;
using ScholarshipService.API.Middleware;
using ScholarshipService.Application;
using ScholarshipService.Infrastructure;
using ScholarshipService.Infrastructure.Data;
using Serilog;
using System.Text;

// ─── Serilog bootstrap ────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// ─── Serilog full configuration ───────────────────────────────────────────────
builder.Host.UseSerilog((ctx, services, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .ReadFrom.Services(services)
       .Enrich.FromLogContext()
       .WriteTo.Console());

// ─── Application Insights ─────────────────────────────────────────────────────
var appInsightsKey = builder.Configuration["ApplicationInsights:InstrumentationKey"];
if (!string.IsNullOrWhiteSpace(appInsightsKey))
    builder.Services.AddApplicationInsightsTelemetry();

// ─── Application & Infrastructure DI ─────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── JWT Authentication / Authorization ───────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ScholarshipServiceDefaultSecretKey!!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ScholarshipService",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ScholarshipServiceClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("HrPolicy", policy => policy.RequireRole("HR", "Admin"));
});

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    options.AddPolicy("Production", policy =>
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
              .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

// ─── API Versioning ───────────────────────────────────────────────────────────
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-API-Version"),
        new QueryStringApiVersionReader("api-version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ─── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Scholarship Service API",
        Version = "v1",
        Description = "ERP Microservice — Scholarship Management API (DDD · CQRS · GraphQL · RabbitMQ)"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Format: Bearer {token}",
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
            []
        }
    });
});

// ─── GraphQL (HotChocolate) ───────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ScholarshipQuery>()
    .AddMutationType<ScholarshipMutation>()
    .AddSubscriptionType<ScholarshipSubscription>()
    .AddType<ScholarshipMainType>()
    .AddType<ScholarshipDetailType>()
    .AddType<ScholarshipAmountType>()
    .AddInMemorySubscriptions();

// ─── HTTP Client with standard resilience (retry + circuit breaker) ───────────
builder.Services.AddHttpClient("ApiGateway", c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiGateway:BaseUrl"] ?? "http://localhost:5000"));

// ─── Health checks ─────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks();

// ─── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Enable Swagger in all environments for API documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scholarship Service v1");
    c.RoutePrefix = "swagger";
});

app.UseWebSockets();
app.UseRouting();
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");
app.UseAuthentication();
app.UseAuthorization();

// ─── Controller endpoints ─────────────────────────────────────────────────────
app.MapControllers();

// ─── Minimal API endpoints ────────────────────────────────────────────────────
app.MapScholarshipEndpoints();
app.MapScholarshipAmountEndpoints();

// ─── GraphQL endpoint ─────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ─── Health check ─────────────────────────────────────────────────────────────
app.MapHealthChecks("/health");

// ─── Auto-migrate database on startup ────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScholarshipDbContext>();
    var pendingMigrations = (await db.Database.GetPendingMigrationsAsync()).ToList();
    if (pendingMigrations.Count > 0)
    {
        try
        {
            await db.Database.MigrateAsync();
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 2714)
        {
            // Tables already exist — database was pre-created from SQL scripts.
            // Mark each pending migration as applied so they won't run again.
            var conn = db.Database.GetDbConnection();
            await conn.OpenAsync();
            foreach (var migrationId in pendingMigrations)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = @id) " +
                    "INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (@id, @ver)";
                var pId = cmd.CreateParameter(); pId.ParameterName = "@id"; pId.Value = migrationId;
                var pVer = cmd.CreateParameter(); pVer.ParameterName = "@ver"; pVer.Value = "10.0.3";
                cmd.Parameters.Add(pId);
                cmd.Parameters.Add(pVer);
                await cmd.ExecuteNonQueryAsync();
            }
            await conn.CloseAsync();
            Log.Warning("Database tables already exist. Marked {Count} migration(s) as applied.", pendingMigrations.Count);
        }
    }
}

app.Run();
