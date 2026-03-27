using System.Text;
using DealTicketing.API.Endpoints;
using DealTicketing.API.GraphQL;
using DealTicketing.API.Middleware;
using DealTicketing.API.Services;
using DealTicketing.Application;
using DealTicketing.Application.Common.Interfaces;
using DealTicketing.Infrastructure;
using DealTicketing.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ── Core Services ────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

// ── Application + Infrastructure ─────────────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── JWT Auth ─────────────────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DealTicketing API",
        Version = "v1",
        Description = "Financial Deal Ticketing Microservice – manages FX, derivatives, and swaps."
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your-token}",
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

// ── GraphQL (Hot Chocolate) ───────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<DealQuery>()
    .AddMutationType<DealMutation>()
    .AddAuthorization()
    .BindRuntimeType<char, HotChocolate.Types.StringType>();

// ── Health Checks ─────────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DealTicketingDb") ?? string.Empty,
        name: "sqlserver",
        tags: ["db", "ready"]);

var app = builder.Build();

// ── Middleware Pipeline ────────────────────────────────────────────────────────
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DealTicketing API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapDealEndpoints();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

// ── EF Migrations auto-apply (dev only) ───────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    await DbInitializer.SeedAsync(app.Services);
}

app.Run();
