using WebsiteContentService.Application.Behaviors;
using WebsiteContentService.Application.Commands.Pages;
using WebsiteContentService.Application.Mappings;
using WebsiteContentService.API.Configuration;
using WebsiteContentService.API.Endpoints;
using WebsiteContentService.API.GraphQL;
using WebsiteContentService.API.Middleware;
using WebsiteContentService.Infrastructure;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using Scalar.AspNetCore;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/website-content-service.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Controllers & OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddExternalServices(builder.Configuration);

// Application (MediatR, AutoMapper, Validation)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    typeof(CreateWebsitePageCommand).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<WebsiteContentMappingProfile>());
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// HTTP Client & Resilience
builder.Services.AddHttpClient();

// Health Checks
builder.Services.AddHealthChecksConfiguration(connectionString);

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<WebsiteContentQuery>()
    .AddMutationType<WebsiteContentMutation>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Middleware
app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("WebsiteContent Service API");
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecksConfiguration();

// Endpoints
app.MapControllers();
app.MapWebsitePageEndpoints();
app.MapWebsiteNewsEndpoints();
app.MapGraphQL();

// Auth token endpoint
app.MapPost("/api/auth/token", (IConfiguration cfg) =>
{
    var jwtSettings = cfg.GetSection("Jwt");
    var secureKey = Encoding.ASCII.GetBytes(jwtSettings["SecureKey"]!);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "Admin")
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = jwtSettings["Issuer"],
        Audience = jwtSettings["Audience"],
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secureKey),
            SecurityAlgorithms.HmacSha256Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return Results.Ok(new { token = tokenHandler.WriteToken(token), expiresIn = 3600 });
}).WithTags("Auth").AllowAnonymous();

// RabbitMQ test endpoint
app.MapGet("/api/rabbitmq/test", (HttpContext ctx) =>
{
    var rabbitConnection = ctx.RequestServices.GetService<IConnection>();
    var connected = rabbitConnection?.IsOpen == true;
    return Results.Ok(new { status = connected ? "Connected" : "Disconnected", service = "RabbitMQ" });
}).WithTags("Infrastructure").AllowAnonymous();

// Database initialization
try
{
    var scope = app.Services.CreateScope();
    await scope.ServiceProvider.InitializeDatabaseAsync();
    Log.Information("Database initialized successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred during database initialization");
}

app.Run();
