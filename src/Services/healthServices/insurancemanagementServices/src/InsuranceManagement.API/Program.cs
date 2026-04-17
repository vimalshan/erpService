using Microsoft.EntityFrameworkCore;
using InsuranceManagement.Infrastructure.Data;
using InsuranceManagement.Infrastructure.Repositories;
using InsuranceManagement.Infrastructure.Extensions;
using InsuranceManagement.Infrastructure.HealthChecks;
using Serilog;
using InsuranceManagement.API.Configuration;
using InsuranceManagement.API.Middleware;
using InsuranceManagement.Infrastructure.Resilience;
using InsuranceManagement.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");

builder.Services.AddDbContext<InsuranceManagementDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(typeof(InsuranceManagementDbContext).Assembly.FullName);
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    }));

// Register repositories and UoW
builder.Services.AddScoped<IInsuranceManagementUnitOfWork, EFUnitOfWork>();

// CQRS and MediatR
builder.Services.AddInsuranceManagementApplicationServices();

// Resilience Policies
builder.Services.AddResiliencePolicies(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapperConfiguration();

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Insurance Management API", 
        Version = "v1",
        Description = "API for managing insurance enrollments, plans, and claims"
    });
});

// GraphQL - register types in DI so constructor injection of IMediator works
builder.Services.AddScoped<InsuranceManagement.API.GraphQL.InsuranceQuery>();
builder.Services.AddScoped<InsuranceManagement.API.GraphQL.InsuranceMutation>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<InsuranceManagement.API.GraphQL.InsuranceQuery>()
    .AddMutationType<InsuranceManagement.API.GraphQL.InsuranceMutation>()
    .AllowIntrospection(true)
    .AddAuthorization();

// Health checks
builder.Services.AddHealthChecks()
    .AddCustomHealthChecks(builder.Configuration);

// Health checks UI
var enableRabbitMq = bool.TryParse(builder.Configuration["RabbitMQ:Enabled"], out var b) && b;
if (enableRabbitMq)
{
    builder.Services.AddRabbitMqConsumers(builder.Configuration);
}

// CORS
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

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InsuranceManagementDbContext>();
    dbContext.Database.Migrate();
    
    // Seed initial data
    await InsuranceManagement.Infrastructure.Seeders.InsuranceDataSeeder.SeedInsurancePlansAsync(dbContext);
}

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance Management API v1");
});

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Health checks endpoint
app.MapHealthChecks("/health");

// GraphQL endpoint
app.MapGraphQL("/graphql");

// Minimal APIs endpoint
app.MapInsuranceEndpoints();

app.MapControllers();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
