using ActionService.Data;
using ActionService.Extensions;
using ActionService.GraphQL.Mutations;
using ActionService.GraphQL.Queries;
using ActionService.GraphQL.Types;
using ActionService.Infrastructure.Data;
using ActionService.Middleware;
using ActionService.Repositories;
using ActionService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Legacy service registrations kept for backward compatibility
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IActionRepository, ActionRepository>();
// ActionService now uses ActionDbContext (EF LINQ) instead of Dapper stored procedures
builder.Services.AddScoped<IActionService, ActionService.Services.ActionService>();

// New layered architecture services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddMessagingServices(builder.Configuration);
builder.Services.AddHealthCheckServices(builder.Configuration);

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32Characters!";
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("role", "admin"));
    options.AddPolicy("Auditor", policy => policy.RequireClaim("role", "auditor", "admin"));
    options.AddPolicy("User", policy => policy.RequireClaim("role", "user", "auditor", "admin"));
});

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<ActionType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddErrorFilter<ActionService.GraphQL.GraphQLErrorFilter>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Apply migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ActionService.Infrastructure.Data.ActionDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await ActionService.Infrastructure.Data.ActionDataSeeder.SeedAsync(db, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database migration/seeding failed");
    }
}

// Middleware pipeline
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// Minimal API endpoints
app.MapGet("/api/actions/minimal", async (ActionService.Domain.Interfaces.IActionRepository repo) =>
    Results.Ok(await repo.GetAllAsync()));

app.MapGet("/api/actions/minimal/{id}", async (int id, ActionService.Domain.Interfaces.IActionRepository repo) =>
{
    var action = await repo.GetByIdAsync(id);
    return action is not null ? Results.Ok(action) : Results.NotFound();
});

app.Run();
