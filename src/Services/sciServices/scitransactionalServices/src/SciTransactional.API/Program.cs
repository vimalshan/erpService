using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using SciTransactional.API.Auth;
using SciTransactional.API.GraphQL.Mutations;
using SciTransactional.API.GraphQL.Queries;
using SciTransactional.API.GraphQL.Types;
using SciTransactional.API.HealthChecks;
using SciTransactional.API.Middleware;
using SciTransactional.API.MinimalApis;
using SciTransactional.Application;
using SciTransactional.Infrastructure;
using SciTransactional.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// JWT Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

// Authentication
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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
    };
});
builder.Services.AddAuthorization();

// Application & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SciTransactional API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TransactionalQueryType>()
    .AddMutationType<TransactionalMutationType>()
    .AddType<NavigationGqlType>()
    .AddType<NormsMainGqlType>()
    .AddType<NormsMasterGqlType>()
    .AddType<AdvanceLicenseGqlType>()
    .AddType<EntitlementGqlType>()
    .AddType<AutoMailStatusGqlType>()
    .AddType<OrderMapGqlType>()
    .AddType<DirectEntryGqlType>()
    .AddAuthorization();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddDbContextCheck<SciTransactionalDbContext>("ef-database");

// Circuit Breaker (for external HTTP calls)
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)));

var app = builder.Build();

// Middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SciTransactional API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Minimal APIs
app.MapTransactionalEndpoints();

// GraphQL
app.MapGraphQL();

// Health Checks
app.MapHealthChecks("/health");

// Apply pending migrations (skip if tables already exist)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<SciTransactionalDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 2714)
    {
        // Table already exists – safe to ignore on pre-existing database
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("Migration skipped: tables already exist in database.");
    }
}

app.Run();
