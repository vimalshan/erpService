using AgencyService.Api.Authentication;
using AgencyService.Api.Endpoints;
using AgencyService.Api.GraphQL;
using AgencyService.Api.HealthChecks;
using AgencyService.Api.Middleware;
using AgencyService.Application.Commands;
using AgencyService.Infrastructure;
using AgencyService.Infrastructure.Data;
using MediatR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=0";

// Services
builder.Services.AddLogging();
builder.Services.AddControllers();

// Infrastructure
builder.Services.AddInfrastructure(connectionString, builder.Environment);
builder.Services.AddAdvancedFeatures(builder.Configuration);

// Application - MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateAgencyCommand)));

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agency Service API",
        Version = "v1",
        Description = "Travel Agency Management Microservice"
    });
    
    // Add JWT Bearer authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// GraphQL - HotChocolate
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .RegisterService<IMediator>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<ApiHealthCheck>("api");

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

// Middleware
app.UseExceptionHandlingMiddleware();
app.UseRequestLoggingMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agency Service API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Health Checks Endpoint
app.MapHealthChecks("/health");

// Minimal APIs - Endpoints
app.MapAgencyEndpoints();
app.MapVendorEndpoints();
app.MapAirlineEndpoints();

// Controllers
app.MapControllers();

// GraphQL
app.MapGraphQL("/graphql");

// Database Initialization
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AgencyDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        var seedService = scope.ServiceProvider.GetRequiredService<SeedDataService>();
        await seedService.SeedAsync();
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogWarning($"Database initialization or seeding failed: {ex.Message}. Application will continue without seed data.");
}

app.Run();
