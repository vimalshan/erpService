using EmailNotification.API.Middleware;
using EmailNotification.Application;
using EmailNotification.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("DefaultConnection not found in configuration");

// Add services to the container
builder.Services.AddControllers();

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

// Add application layer services
builder.Services.AddApplicationServices();

// Add infrastructure layer services
builder.Services.AddInfrastructureServices(connectionString);

// Add RabbitMQ messaging services (only in non-development environments)
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddRabbitMqServices(builder.Configuration);
}

// Add resilience policies
builder.Services.AddResiliencePolicies();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "Database");

// Add JWT Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = false,
            ValidateLifetime = true
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Configure background service error handling
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

// Build the application
var app = builder.Build();

// Initialize database (migrations and seeding)
await InitializeDatabaseAsync(app);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    // Add Swagger/OpenAPI UI endpoint
    app.MapGet("/swagger/index.html", () => 
    {
        var html = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Notification Service API</title>
    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/swagger-ui-dist@4/swagger-ui.css"">
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: sans-serif;
            background-color: #fafafa;
        }
    </style>
</head>
<body>
    <div id=""swagger-ui""></div>
    <script src=""https://cdn.jsdelivr.net/npm/swagger-ui-dist@4/swagger-ui-bundle.js""></script>
    <script src=""https://cdn.jsdelivr.net/npm/swagger-ui-dist@4/swagger-ui-standalone-preset.js""></script>
    <script>
        window.addEventListener('load', function() {
            window.ui = SwaggerUIBundle({
                url: ""/openapi.json"",
                dom_id: '#swagger-ui',
                presets: [
                    SwaggerUIBundle.presets.apis,
                    SwaggerUIStandalonePreset
                ],
                layout: ""StandaloneLayout"",
                onFailure: function(data) {
                    console.error('Failed to load spec:', data);
                }
            });
        });
    </script>
</body>
</html>";
        return Results.Content(html, "text/html");
    }).WithName("SwaggerUI");
    
    // Add OpenAPI specification endpoint
    app.MapGet("/openapi.json", () => 
    {
        var openApiSpec = new
        {
            openapi = "3.0.1",
            info = new
            {
                title = "Email Notification Service API",
                version = "1.0.0",
                description = "REST API for managing email notification types and recipients in the ERP system",
                contact = new
                {
                    name = "Development Team"
                }
            },
            servers = new object[] 
            {
                new { url = "http://localhost:5031", description = "Development" }
            },
            paths = new
            {
                __index = new object(),
            },
            components = new
            {
                schemas = new
                {
                    EmailTypeDto = new
                    {
                        type = "object",
                        properties = new
                        {
                            id = new { type = "integer", description = "Email type ID" },
                            emailName = new { type = "string", description = "Name of the email type" },
                            emailType = new { type = "string", description = "Email type (Daily/Event)" },
                            emailProcName = new { type = "string", description = "Database stored procedure name" },
                            recipients = new { type = "array", description = "List of recipients" },
                            modifiedBy = new { type = "integer", description = "Last modified by user ID" },
                            modifiedOn = new { type = "string", format = "date-time", description = "Last modification timestamp" }
                        }
                    }
                },
                securitySchemes = new
                {
                    BearerAuth = new
                    {
                        type = "http",
                        scheme = "bearer",
                        description = "JWT Bearer token"
                    }
                }
            }
        };
        return Results.Json(openApiSpec);
    }).WithName("OpenAPISpec");
}

// Use exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint
app.MapHealthChecks("/health");

// Map controllers
app.MapControllers();

app.Run();

/// <summary>
/// Initializes the database by applying migrations and seeding data
/// </summary>
async Task InitializeDatabaseAsync(WebApplication application)
{
    try
    {
        using var scope = application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EmailNotification.Infrastructure.Data.EmailNotificationDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<EmailNotification.Infrastructure.Data.IDataSeeder>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully");

        logger.LogInformation("Seeding database with initial data...");
        await seeder.SeedAsync();
        logger.LogInformation("Database seeding completed");
    }
    catch (Exception ex)
    {
        var logger = application.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error initializing database");
        throw;
    }
}

