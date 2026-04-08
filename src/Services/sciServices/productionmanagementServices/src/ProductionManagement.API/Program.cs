using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Polly;
using ProductionManagement.API.Endpoints;
using ProductionManagement.API.GraphQL;
using ProductionManagement.API.GraphQL.Types;
using ProductionManagement.API.Middleware;
using ProductionManagement.Application;
using ProductionManagement.Infrastructure;
using ProductionManagement.Infrastructure.Persistence;
using ProductionManagement.Infrastructure.Seed;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ProductionManagement_SuperSecret_Key_12345678!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ProductionManagement";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ProductionManagement";

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Production Management API",
        Version = "v1",
        Description = "Production Management Microservice API"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
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

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<ProductionPlantType>()
    .AddType<ProductionPlanType>()
    .AddType<NormsMainType>()
    .AddType<ProductionPlanEntryType>()
    .AddType<NormsMasterType2>()
    .AddType<ProductionPlantProductMapType>()
    .AddType<MamProductionDetType>()
    .AddType<MamProductionMapType>()
    .AddProjections()
    .AddFiltering(c => c.AddDefaults().BindRuntimeType<char, HotChocolate.Data.Filters.StringOperationFilterInputType>().BindRuntimeType<char?, HotChocolate.Data.Filters.StringOperationFilterInputType>())
    .AddSorting()
    .BindRuntimeType<char, HotChocolate.Types.StringType>()
    .BindRuntimeType<char?, HotChocolate.Types.StringType>()
    .RegisterDbContextFactory<ProductionManagementDbContext>();

// Circuit Breaker with Polly
builder.Services.AddHttpClient("ExternalService")
    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30)))
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))));

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" })
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
        tags: new[] { "self" });

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Production Management API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMinimalApiEndpoints();
app.MapGraphQL();
app.MapHealthChecks("/health");

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductionManagementDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.SeedAsync(context);
}

await app.RunAsync();
