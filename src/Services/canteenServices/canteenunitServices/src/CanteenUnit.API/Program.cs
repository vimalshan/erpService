using CanteenUnit.API.GraphQL.Mutations;
using CanteenUnit.API.GraphQL.Queries;
using CanteenUnit.API.HealthChecks;
using CanteenUnit.API.Middleware;
using CanteenUnit.API.MinimalApis;
using CanteenUnit.Application;
using CanteenUnit.Infrastructure;
using CanteenUnit.Infrastructure.Persistence.Seeders;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure ────────────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ─── JWT Authentication ───────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
            ValidIssuer = jwtIssuer,
            ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// ─── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ─── Swagger ─────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CanteenUnit API",
        Version = "v1",
        Description = "ERP Canteen Unit Microservice"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// ─── GraphQL ─────────────────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CanteenUnitQuery>()
    .AddMutationType<CanteenUnitMutation>()
    .BindRuntimeType<char, HotChocolate.Types.StringType>()
    .BindRuntimeType<char?, HotChocolate.Types.StringType>();

// ─── Health Checks ────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddDbContextCheck<CanteenUnit.Infrastructure.Persistence.ApplicationDbContext>("ef-core");

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CanteenUnit API v1"));

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// REST Controllers
app.MapControllers();

// Minimal API v2 routes
app.MapCanteenUnitEndpoints();

// GraphQL
app.MapGraphQL("/graphql");

// Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Seed database
await DataSeeder.SeedAsync(app.Services);

app.Run();
