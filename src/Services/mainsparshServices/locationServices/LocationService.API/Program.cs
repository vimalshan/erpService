using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MediatR;
using RabbitMQ.Client;
using LocationService.Infrastructure.Persistence;
using LocationService.Infrastructure.Persistence.Repositories;
using LocationService.Infrastructure.Persistence.Seeds;
using LocationService.Infrastructure.ExternalServices;
using LocationService.Infrastructure.Messaging;
using LocationService.Infrastructure.Caching;
using LocationService.Application.Behaviors;
using LocationService.Application.Mappings;
using LocationService.Domain.Entities;
using LocationService.API.Security;
using LocationService.API.Middleware;
using LocationService.API.GraphQL;
using LocationService.API.Endpoints;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Location Service API",
        Version = "v1",
        Description = "Location, Room, and Resource Management Microservice",
        Contact = new OpenApiContact { Name = "Architecture Team" }
    });

    // JWT bearer in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Database
builder.Services.AddDbContext<LocationServiceDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.MigrationsAssembly("LocationService.Infrastructure")
    ));

// Unit of Work and Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomResourceRepository, RoomResourceRepository>();

// MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(EntityMappingProfile).Assembly);
    config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<EntityMappingProfile>());

// RabbitMQ
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq");
var rabbitMqHost = rabbitMqSettings["Host"] ?? "localhost";
var rabbitMqPort = int.Parse(rabbitMqSettings["Port"] ?? "5672");
var rabbitMqUser = rabbitMqSettings["User"] ?? "guest";
var rabbitMqPassword = rabbitMqSettings["Password"] ?? "guest";

var connectionFactory = new ConnectionFactory
{
    HostName = rabbitMqHost,
    Port = rabbitMqPort,
    UserName = rabbitMqUser,
    Password = rabbitMqPassword
};

builder.Services.AddSingleton<IConnection>(sp =>
{
    try
    {
        return connectionFactory.CreateConnection();
    }
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "RabbitMQ is not available. Messaging features will not work.");
        throw;
    }
});
builder.Services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();

// Caching
if (builder.Configuration.GetValue<bool>("CacheSettings:UseRedis"))
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost";
    builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(
        StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnectionString));
    builder.Services.AddSingleton<ICacheService, RedisCacheService>();
}
else
{
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<ICacheService, MemoryCacheService>();
}

// Azure Blob Storage
builder.Services.AddScoped(sp =>
{
    var blobStorageSettings = builder.Configuration.GetSection("BlobStorage");
    var connectionStr = blobStorageSettings["ConnectionString"];
    return new BlobServiceClient(new Uri(connectionStr!));
});
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Dapper
builder.Services.AddScoped<IDapperRepository>(sp =>
    new DapperRepository(connectionString));

// JWT Token Service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddLocationServiceSchema();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<LocationServiceDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Migrate database and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LocationServiceDbContext>();
    await dbContext.Database.MigrateAsync();
    await SeedData.SeedDatabaseAsync(dbContext);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Location Service API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapLocationEndpoints();
app.MapGraphQL();
app.MapHealthChecks("/health");

// Auth token endpoint
app.MapPost("/api/auth/token", (IJwtTokenService tokenService) =>
{
    var token = tokenService.GenerateToken(1, "admin@test.com", ["Admin"]);
    return Results.Ok(new { token, expiresIn = 3600 });
}).AllowAnonymous();

// RabbitMQ test endpoint
app.MapGet("/api/rabbitmq/test", (IServiceProvider sp) =>
{
    try
    {
        var connection = sp.GetRequiredService<RabbitMQ.Client.IConnection>();
        return Results.Ok(new { service = "RabbitMQ", status = connection.IsOpen ? "Connected" : "Disconnected" });
    }
    catch
    {
        return Results.Ok(new { service = "RabbitMQ", status = "Disconnected" });
    }
}).AllowAnonymous();

app.Run();
