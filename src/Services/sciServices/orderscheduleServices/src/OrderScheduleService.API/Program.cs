using OrderScheduleService.API.Middleware;
using OrderScheduleService.API.Services;
using OrderScheduleService.Application.Mapping;
using OrderScheduleService.Infrastructure;
using OrderScheduleService.Infrastructure.Persistence;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Order Scheduling Service API",
        Version = "v1",
        Description = "Microservice for managing order scheduling and fulfillment"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<OrderScheduleService.API.GraphQL.Query>()
    .AddMutationType<OrderScheduleService.API.GraphQL.Mutation>()
    .AddSubscriptionType<OrderScheduleService.API.GraphQL.Subscription>();

// Add MediatR
var executingAssembly = typeof(Program).Assembly;
var applicationAssembly = typeof(OrderScheduleService.Application.CommandHandlers.CreateTiedOrderCommandHandler).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(executingAssembly, applicationAssembly);
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(connectionString ?? throw new InvalidOperationException("Connection string not found"));

// Add JWT Authentication
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString ?? string.Empty, name: "Database", failureStatus: HealthStatus.Unhealthy)
    .AddCheck("API", () => HealthCheckResult.Healthy(), tags: new[] { "live" });

// Add JWT Token Service
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Add Polly Resilience Policies
builder.Services.AddHttpClient("Default", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add RabbitMQ Configuration (placeholder for now)
builder.Services.AddSingleton<OrderScheduleService.IntegrationEvents.RabbitMqConfiguration>();

var app = builder.Build();

// Migrate and seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderScheduleService.Infrastructure.Persistence.OrderScheduleDbContext>();
    await dbContext.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(dbContext);
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Add Custom Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Add Health Checks Endpoint
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

// Map GraphQL
app.MapGraphQL();

// Map Minimal APIs
OrderScheduleService.API.MinimalApiExtensions.MapMinimalApis(app);

app.Run();
