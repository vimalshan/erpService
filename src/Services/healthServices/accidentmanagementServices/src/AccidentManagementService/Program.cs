using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AccidentManagementService.Infrastructure.Persistence;
using AccidentManagementService.Infrastructure.Data;
using AccidentManagementService.Infrastructure.EventBus;
using AccidentManagementService.Infrastructure.EventBus.Consumers;
using AccidentManagementService.GraphQL;
using MassTransit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment;

// DbContext configuration - CRITICAL for EF migrations
builder.Services.AddDbContext<AccidentManagementDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("HealthDb")
        ?? throw new InvalidOperationException("Connection string 'HealthDb' not configured in appsettings.json");

    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly("AccidentManagementService"));

    if (environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<DataSeedingService>();

// JWT Authentication configuration
var authSettings = configuration.GetSection("Authentication");
var secretKey = authSettings["SecretKey"];
var issuer = authSettings["Issuer"];
var audience = authSettings["Audience"];

if (!string.IsNullOrEmpty(secretKey) && secretKey.Length >= 32)
{
    var key = Encoding.ASCII.GetBytes(secretKey);
    
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = !string.IsNullOrEmpty(issuer),
            ValidIssuer = issuer,
            ValidateAudience = !string.IsNullOrEmpty(audience),
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
}

// Swagger configuration
var enableSwagger = configuration.GetValue<bool>("AppSettings:EnableSwagger", true);
if (enableSwagger)
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Accident Management Service API",
            Version = "v1",
            Description = "API for managing accident records and incidents"
        });
        
        // Add JWT Security Definition to Swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });
}

// GraphQL configuration using HotChocolate
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<AccidentType>()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = builder.Environment.IsDevelopment());

// Health Checks
builder.Services.AddHealthChecks();

// RabbitMQ - raw publisher
builder.Services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();

// MassTransit + RabbitMQ consumers
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AccidentReportCreatedConsumer>();
    x.AddConsumer<AccidentStatusChangedConsumer>();
    x.AddConsumer<AccidentSeverityChangedConsumer>();
    x.AddConsumer<AccidentDetailsUpdatedConsumer>();
    x.AddConsumer<AccidentReportDeletedConsumer>();
    x.AddConsumer<AccidentReportRestoredConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitPort = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
        var rabbitUser = configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(rabbitHost, (ushort)rabbitPort, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        cfg.ReceiveEndpoint("accident_queue", ep =>
        {
            ep.ConfigureConsumers(context);
        });
    });
});

var app = builder.Build();

// Log the environment
app.Logger.LogInformation("Application starting in {Environment} mode", app.Environment.EnvironmentName);

// Always seed database on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var seedingService = scope.ServiceProvider.GetRequiredService<DataSeedingService>();
        await seedingService.SeedAsync();
        app.Logger.LogInformation("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Database seeding failed");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger middleware
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Accident Management Service API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
