using EmployeeService.API.Middleware;
using EmployeeService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Get configuration
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=0";

// Add services to the container
builder.Services.AddControllers();

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();

// Add Swagger/OpenAPI  
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    {
        Title = "Employee Service API",
        Version = "v1",
        Description = "Employee Management Microservice API"
    });
    
    // Enable annotations
    c.EnableAnnotations();
});

// Add JWT Authentication
var jwtKey = configuration["Jwt:Key"] ?? "YourSuperSecretKeyChangeThisInProduction12345ShouldBeAtLeast32Chars";
var jwtIssuer = configuration["Jwt:Issuer"] ?? "EmployeeService";
var jwtAudience = configuration["Jwt:Audience"] ?? "EmployeeServiceAPI";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new { message = "Unauthorized" });
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new { message = "Forbidden" });
            }
        };
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("HRPolicy", policy => policy.RequireRole("HR", "Admin"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager", "HR", "Admin"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy("AllowSpecific", builder =>
    {
        builder.WithOrigins("https://localhost:3000", "http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(connectionString, configuration);
builder.Services.AddApplicationServices();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "Database");

// Add GraphQL
builder.Services.AddGraphQLServer()
    .BindRuntimeType<char, HotChocolate.Types.StringType>()
    .AddQueryType<EmployeeService.API.GraphQL.Query>()
    .AddMutationType<EmployeeService.API.GraphQL.Mutation>()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Service API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Use custom middleware
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Map Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { });

// Map Controllers
app.MapControllers();

// Map GraphQL
app.MapGraphQL("/graphql");

// Swagger endpoint
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html", permanent: false);
    return Task.CompletedTask;
});

// Initialize database
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeService.Infrastructure.Data.EmployeeServiceDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Initializing database...");
        await EmployeeService.Infrastructure.Data.DbInitializer.InitializeAsync(dbContext, logger);
        
        // Only seed data if database was just created (no existing data)
        if (!dbContext.Employees.Any())
        {
            logger.LogInformation("Seeding database with sample data...");
            await EmployeeService.Infrastructure.Data.DbInitializer.SeedDataAsync(dbContext, logger);
        }
        else
        {
            logger.LogInformation("Database already contains data. Skipping seed.");
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during database initialization. Continuing with API startup...");
}

app.Run();
