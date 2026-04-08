using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PurchaseSalesService.API.Middleware;
using PurchaseSalesService.API.MinimalApis;
using PurchaseSalesService.API.Services;
using PurchaseSalesService.Application;
using PurchaseSalesService.Application.Common.Interfaces;
using PurchaseSalesService.Infrastructure;
using PurchaseSalesService.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ─── Host options ─────────────────────────────────────────────────────────────
// Prevent a faulting BackgroundService (e.g. RabbitMQ unavailable) from stopping the host.
builder.Services.Configure<HostOptions>(o =>
    o.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

// ─── Application & Infrastructure ────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ─── Swagger / OpenAPI ───────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PurchaseSales Service API",
        Version = "v1",
        Description = "ERP Microservice — Purchase & Sales Module"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ─── JWT Authentication ───────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key not configured.");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ─── GraphQL ─────────────────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<PurchaseSalesService.API.GraphQL.Query>()
    .AddMutationType<PurchaseSalesService.API.GraphQL.Mutation>()
    .BindRuntimeType<char, HotChocolate.Types.StringType>()
    .BindRuntimeType<char?, HotChocolate.Types.StringType>();

// ─── Health Checks ────────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" })
    .AddRabbitMQ(
        sp =>
        {
            var cfg = sp.GetRequiredService<IConfiguration>();
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = cfg["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(cfg["RabbitMQ:Port"] ?? "5672"),
                UserName = cfg["RabbitMQ:Username"] ?? "guest",
                Password = cfg["RabbitMQ:Password"] ?? "guest"
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        },
        name: "rabbitmq",
        tags: new[] { "messaging" });

// ─── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Database Migration & Seed ───────────────────────────────────────────────
await DatabaseInitializer.InitializeAsync(app.Services);

// ─── Middleware Pipeline ──────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PurchaseSales Service v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// Minimal API endpoints
app.MapPurchaseEndpoints();
app.MapSaleEndpoints();

app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
