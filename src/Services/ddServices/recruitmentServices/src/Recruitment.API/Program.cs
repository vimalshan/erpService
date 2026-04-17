using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application;
using Recruitment.Infrastructure;
using Recruitment.API.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container
builder.Host.UseSerilog();

// Register Application and Infrastructure services
builder.Services.AddApplication();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");
builder.Services.AddInfrastructure(connectionString);

// Register Token Service for JWT generation
builder.Services.AddScoped<Recruitment.API.Services.ITokenService, Recruitment.API.Services.JwtTokenService>();

// Add Controllers
builder.Services.AddControllers();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings.GetValue<string>("Secret") ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLongForHS256Algorithm";
var issuer = jwtSettings.GetValue<string>("Issuer") ?? "RecruitmentService";
var audience = jwtSettings.GetValue<string>("Audience") ?? "RecruitmentServiceClients";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add Swagger/OpenAPI (basic configuration)
builder.Services.AddSwaggerGen();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "Database");

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply migrations automatically and seed data
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<Recruitment.Infrastructure.Persistence.RecruitmentDbContext>();
        Console.WriteLine("=== Applying Database Migrations ===");
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("✅ Migrations applied successfully");
        
        // Seed initial data in development environment
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("=== Seeding Sample Data ===");
            await Recruitment.Infrastructure.Persistence.SeedDataService.SeedAsync(dbContext);
            Console.WriteLine("✅ Database seeding completed");
        }
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"✗ Database initialization failed: {ex.Message}");
    Console.ResetColor();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruitment Service API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Map health checks endpoint
app.MapHealthChecks("/health");

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

// Map Controllers
app.MapControllers();

Console.WriteLine("\n========================================");
Console.WriteLine("🚀 Recruitment Microservice Started");
Console.WriteLine("========================================");
Console.WriteLine("📄 Swagger UI: https://localhost:7095/swagger");
Console.WriteLine("📊 GraphQL: https://localhost:7095/graphql");
Console.WriteLine("🏥 Health: https://localhost:7095/health");
Console.WriteLine("🔐 Login: POST https://localhost:7095/api/auth/login");
Console.WriteLine("========================================\n");

app.Run();
