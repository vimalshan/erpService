using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ReviewService.Application;
using ReviewService.Infrastructure;
using ReviewService.Infrastructure.Data.Seeds;
using ReviewService.Infrastructure.Consumers;
using ReviewService.API.Middleware;
using ReviewService.API.MinimalApis;
using ReviewService.API.GraphQL;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ── Application & Infrastructure Layers ─────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(configuration);

// ── Controllers + Swagger/OpenAPI ───────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReviewService API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// ── JWT Authentication ───────────────────────────────────────────────────────
var jwtKey = configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// ── GraphQL (HotChocolate) ───────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ReviewQuery>()
    .AddMutationType<ReviewMutation>()
    .AddAuthorization();

// ── Health Checks ────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db", "sql"])
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

// ── RabbitMQ Consumer ────────────────────────────────────────────────────────
builder.Services.AddHostedService<FeedbackSubmittedConsumer>();

// ── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(opts =>
    opts.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed database ────────────────────────────────────────────────────────────
await DatabaseSeeder.SeedAsync(app.Services);

// ── Middleware Pipeline ──────────────────────────────────────────────────────
app.UseGlobalExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReviewService v1"));
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ── Controllers ──────────────────────────────────────────────────────────────
app.MapControllers();

// ── Minimal API Endpoints ────────────────────────────────────────────────────
app.MapReviewEndpoints();

// ── GraphQL ──────────────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ── Health Checks ────────────────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.Run();

