using CompensationService.Application.Behaviors;
using CompensationService.Application.Mappings;
using CompensationService.API.Configuration;
using CompensationService.API.Middleware;
using CompensationService.API.Endpoints;
using CompensationService.API.GraphQL;
using CompensationService.Infrastructure;
using MediatR;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/compensation-service.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Infrastructure services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddExternalServices(builder.Configuration);

// Application services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(CreateCompensationGradeCommand).Assembly));
builder.Services.AddAutoMapper(typeof(CompensationGradeMappingProfile));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// API services
builder.Services.AddJwtAuthentication(builder.Configuration);
// HTTP client configuration
builder.Services.AddHttpClient();
builder.Services.AddHealthChecksConfiguration(connectionString);

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CompensationGradeQuery>()
    .AddMutationType<CompensationGradeMutation>();

var app = builder.Build();

// Middleware
app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecksConfiguration();

app.MapControllers();
app.MapCompensationGradeEndpoints();
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

// Initialize database
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
