using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthorizationService.API.Extensions;
using AuthorizationService.API.HealthChecks;
using AuthorizationService.API.Middleware;
using AuthorizationService.Application.Mappings;
using AuthorizationService.Application.Behaviors;
using MediatR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "/logs/authorization-service-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// Configuration
var settings = builder.Configuration;
var jwtSettings = settings.GetSection("JwtSettings");

// Infrastructure Services
builder.Services.AddInfrastructureServices(settings);

// MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(MappingProfile));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

// API Services
builder.Services.AddApiServices(settings);

// Health Checks
var connectionString = settings.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");
builder.Services.AddApplicationHealthChecks(connectionString, settings["RabbitMQ:Hostname"]);

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "your-secret-key-change-this-in-production-at-least-32-characters")),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<AuthorizationService.API.GraphQL.QueryType>()
    .AddType<AuthorizationService.API.GraphQL.RightType>()
    .AddType<AuthorizationService.API.GraphQL.UserRightType>()
    .AddType<AuthorizationService.API.GraphQL.TrackerRightType>()
    .AddType<AuthorizationService.API.GraphQL.SpecialInputType>();

var app = builder.Build();

// Seed Database
app.SeedDatabase();

// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<DomainEventPublishingMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization Service API v1");
    if (app.Environment.IsDevelopment())
    {
        options.RoutePrefix = string.Empty;
    }
});

// Health Checks
app.UseApplicationHealthChecks();

// CORS
app.UseCors("AllowAll");

// Authentication & Authorization
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();
app.MapGraphQL("/graphql");

app.Run();
