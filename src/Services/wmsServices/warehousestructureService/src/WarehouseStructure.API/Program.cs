using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WarehouseStructure.Application;
using WarehouseStructure.Infrastructure;
using WarehouseStructure.Infrastructure.Seed;
using WarehouseStructure.API.Endpoints;
using WarehouseStructure.API.GraphQL.Queries;
using WarehouseStructure.API.GraphQL.Mutations;
using WarehouseStructure.API.GraphQL.Types;
using WarehouseStructure.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure DI
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

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
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();

// OpenAPI
builder.Services.AddOpenApi();

// GraphQL (Hot Chocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<WarehouseQuery>()
    .AddMutationType<WarehouseMutation>()
    .AddType<WarehouseGqlType>()
    .AddType<ZoneGqlType>()
    .AddFiltering()
    .AddSorting();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" });

// Polly Circuit Breaker for HttpClient (Polly v8 Resilience Pipeline)
builder.Services.AddHttpClient("ExternalService")
    .AddStandardResilienceHandler();

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// OpenAPI + Scalar API Reference UI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Warehouse Structure API");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers (REST API)
app.MapControllers();

// Map minimal API endpoints
app.MapWarehouseEndpoints();
app.MapZoneEndpoints();

// Map GraphQL
app.MapGraphQL("/graphql");

// Map Health Checks
app.MapHealthChecks("/health");

// Seed Database
await DatabaseSeeder.SeedAsync(app.Services);

app.Run();
