using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FinyearAPI.Data;
using FinyearAPI.Services;
using FinyearAPI.UnitOfWork;
using FinyearAPI.Repositories.Dapper;
using Services.AuthProvider.Authentication;
using Services.AuthProvider.Authorization;
using System.Data;
using Microsoft.Data.SqlClient;
using FinyearAPI.GraphQL.Queries;
using FinyearAPI.GraphQL.Mutations;
using FinyearAPI.GraphQL.Subscriptions;
using FinyearAPI.Application.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ===== DATABASE SETUP =====
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("AdminDbConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=FinyearDB;Trusted_Connection=true;";

builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("FinyearAPI");
        sqlOptions.CommandTimeout(30);
    }));

// ===== DAPPER SETUP =====
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionString));

// ===== SERVICES REGISTRATION =====
builder.Services.AddScoped<IFinancialYearDapperRepository, FinancialYearDapperRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFinancialYearService, FinancialYearService>();

// ===== CONTROLLERS =====
builder.Services.AddControllers();

// ===== AUTHENTICATION =====
var secretKey = configuration["Jwt:SecretKey"] ?? "AuthProviderSuperSecretKey_ChangeInProduction_Min32Chars!";
var issuer = configuration["Jwt:Issuer"] ?? "AuthProvider";
var audience = configuration["Jwt:Audience"] ?? "AuthProviderClients";
var expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");

builder.Services.AddSingleton<IAuthService>(sp =>
    new JwtAuthService(secretKey, issuer, audience, expirationMinutes, sp.GetRequiredService<ILogger<JwtAuthService>>()));

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

// ===== JWT BEARER AUTHENTICATION =====
var jwtKey = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]
    ?? configuration["Jwt:SecretKey"]
    ?? "AuthProviderSuperSecretKey_ChangeInProduction_Min32Chars!");

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
        ValidIssuer = configuration["JwtSettings:Issuer"] ?? configuration["Jwt:Issuer"] ?? "AuthProvider",
        ValidAudience = configuration["JwtSettings:Audience"] ?? configuration["Jwt:Audience"] ?? "AuthProviderClients",
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ===== API DOCUMENTATION =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "FinyearAPI",
        Version = "v1",
        Description = "Financial Year Management API",
        Contact = new()
        {
            Name = "FinyearAPI Support",
            Email = "support@finyear.com"
        }
    });
});

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ===== HEALTH CHECKS =====
builder.Services.AddHealthChecks();

// ===== GRAPHQL =====
builder.Services
    .AddGraphQLServer()
    .AddQueryType<FinancialYearQuery>()
    .AddMutationType<FinancialYearMutation>()
    .AddSubscriptionType<FinancialYearSubscription>()
    .AddInMemorySubscriptions();

// ===== BUILD & RUN =====
var app = builder.Build();

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FinyearAPI v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "FinyearAPI - Swagger Documentation";
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// ===== ROUTING & CONTROLLERS =====
app.UseRouting();
app.MapControllers();

// ===== GRAPHQL ENDPOINT =====
app.MapGraphQL("/graphql");

// ===== HEALTH CHECK =====
app.MapHealthChecks("/health");

// ===== DATABASE INITIALIZATION =====
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    
    // Only run migrations in Development environment
    if (environment.IsDevelopment())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AdminDbContext>();
            logger.LogInformation("Applying database migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database migration");
            throw;
        }
    }
    else
    {
        logger.LogInformation("Skipping database migrations in {Environment} environment.", environment.EnvironmentName);
    }
}

app.Run();
