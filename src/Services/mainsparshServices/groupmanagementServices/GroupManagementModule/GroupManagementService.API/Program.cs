using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using GroupManagementService.Application.Profiles;
using GroupManagementService.Application.Behaviors;
using GroupManagementService.Infrastructure;
using GroupManagementService.Infrastructure.Seeds;
using GroupManagementService.API.Middleware;
using GroupManagementService.API.Security;
using GroupManagementService.API.Configuration;
using GroupManagementService.API.GraphQL;
using GroupManagementService.API.Endpoints;

var corsPolicyName = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var services = builder.Services;
var configuration = builder.Configuration;

// CORS Configuration
services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Controllers
services.AddControllers();

// Swagger/OpenAPI  
services.AddSwaggerGen();

// JWT Authentication
var jwtSecretKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey not configured");
var jwtIssuer = configuration["Jwt:Issuer"] ?? "GroupManagementService";
var jwtAudience = configuration["Jwt:Audience"] ?? "GroupManagementService";

services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });

services.AddAuthorization();

// JWT Token Generator
services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Database and Infrastructure
services.AddInfrastructure(configuration);

// AutoMapper
services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// MediatR
services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(MappingProfile));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
    config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// RabbitMQ Configuration
services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMq"));
services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

// Health Checks
services.AddCustomHealthChecks(configuration);

// GraphQL
services
    .AddGraphQLServer()
    .AddQueryType<GroupQuery>()
    .AddMutationType<GroupMutation>();

// Http Client with Polly
services.AddHttpClient("ExternalAPI")
    .ConfigureHttpClient(client => 
    {
        client.Timeout = TimeSpan.FromSeconds(30);
    });

// Build app
var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Group Management Service API v1");
    });
}

// Health checks
app.MapHealthCheckEndpoints();

// Exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors(corsPolicyName);

// Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Minimal API
app.MapGroupEndpoints();

// GraphQL
app.MapGraphQL("/graphql");

// Auth token endpoint
app.MapPost("/api/auth/token", (IJwtTokenGenerator tokenGenerator) =>
{
    var token = tokenGenerator.GenerateToken(1, "admin@test.com", ["Admin"], TimeSpan.FromHours(1));
    return Results.Ok(new { token, expiresIn = 3600 });
}).AllowAnonymous();

// RabbitMQ test endpoint
app.MapGet("/api/rabbitmq/test", () =>
{
    return Results.Ok(new { status = "Disconnected", service = "RabbitMQ" });
}).AllowAnonymous();

// Database seeding
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<GroupManagementService.Infrastructure.Persistence.GroupManagementDbContext>();
        dbContext.Database.EnsureCreated();
        GroupManagementSeeds.SeedDataAsync(dbContext).Wait();
    }
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Error seeding database on startup");
}

app.Run();
