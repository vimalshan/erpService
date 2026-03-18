using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.CircuitBreaker;
using DocumentService.Application;
using DocumentService.Infrastructure;
using DocumentService.Infrastructure.Data.Seed;
using DocumentService.Infrastructure.Settings;
using DocumentService.API.GraphQL;
using DocumentService.API.Middleware;
using DocumentService.API.MinimalApis;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Application & Infrastructure ─────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── Authentication (JWT) ──────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSettings.Issuer,
            ValidAudience            = jwtSettings.Audience,
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });
builder.Services.AddAuthorization();

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Document Service API",
        Version     = "v1",
        Description = "Loan Document microservice — REST, GraphQL and Minimal API endpoints."
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        Description  = "Enter your JWT token."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    // Use full type names to avoid schema ID collisions across namespaces
    options.CustomSchemaIds(type => type.FullName!.Replace("+", "."));
    // Resolve duplicate action conflicts gracefully
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    // Exclude endpoints that Swashbuckle cannot represent (GraphQL, health checks)
    options.DocInclusionPredicate((_, apiDesc) =>
    {
        var path = apiDesc.RelativePath ?? string.Empty;
        return !path.StartsWith("graphql", StringComparison.OrdinalIgnoreCase)
            && !path.StartsWith("health", StringComparison.OrdinalIgnoreCase);
    });
});

// ── GraphQL (HotChocolate) ────────────────────────────────────────────────────
builder.Services.AddGraphQLServer()
    .AddQueryType<DocumentQuery>()
    .AddMutationType<DocumentMutation>();

// ── Polly Circuit Breaker ─────────────────────────────────────────────────────
builder.Services.AddSingleton<ResiliencePipeline>(_ =>
    new ResiliencePipelineBuilder()
        .AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            FailureRatio      = 0.5,
            SamplingDuration  = TimeSpan.FromSeconds(10),
            MinimumThroughput = 2,
            BreakDuration     = TimeSpan.FromSeconds(30),
            OnOpened = args =>
            {
                Console.WriteLine($"[Circuit Breaker] OPEN — break for {args.BreakDuration}");
                return default;
            },
            OnClosed = _ =>
            {
                Console.WriteLine("[Circuit Breaker] CLOSED — resuming normal operation");
                return default;
            }
        })
        .Build());

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ── Database migration & seed ─────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Service API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ── REST Controllers ──────────────────────────────────────────────────────────
app.MapControllers();

// ── Minimal API endpoints ─────────────────────────────────────────────────────
app.MapLoanDocumentEndpoints();

// ── GraphQL ───────────────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ── Health Checks ─────────────────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy]   = StatusCodes.Status200OK,
        [HealthStatus.Degraded]  = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.Run();

