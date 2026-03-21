using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OrderService.API.Endpoints;
using OrderService.API.GraphQL;
using OrderService.API.GraphQL.Types;
using OrderService.API.Middleware;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Persistence;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// ── Application & Infrastructure DI ──────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── Controllers ──────────────────────────────────────────────────
builder.Services.AddControllers();

// ── JWT Authentication ───────────────────────────────────────────
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();

// ── Swagger / OpenAPI ────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// ── GraphQL (HotChocolate) ───────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<OrderQuery>()
    .AddMutationType<OrderMutation>()
    .AddType<OrderType>()
    .AddType<OrderItemType>();

// ── Polly Circuit Breaker (for external HTTP calls) ──────────────
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)));

// ── Health Checks ────────────────────────────────────────────────
var healthChecks = builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("OrderDb")!,
        name: "sqlserver",
        tags: new[] { "db", "sql" });

if (bool.TryParse(builder.Configuration["RabbitMQ:Enabled"], out var rabbitEnabled) && rabbitEnabled)
{
    healthChecks.AddRabbitMQ(
        sp => new RabbitMQ.Client.ConnectionFactory
        {
            HostName = builder.Configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = builder.Configuration["RabbitMQ:Username"] ?? "guest",
            Password = builder.Configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.TryParse(builder.Configuration["RabbitMQ:Port"], out var p) ? p : 5672
        }.CreateConnectionAsync().GetAwaiter().GetResult(),
        name: "rabbitmq",
        tags: new[] { "messaging" });
}

// ═════════════════════════════════════════════════════════════════
var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => c.SerializeAsV2 = true);
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ── Map endpoints ────────────────────────────────────────────────
app.MapControllers();
app.MapOrderEndpoints();      // Minimal APIs
app.MapGraphQL("/graphql");   // GraphQL via Banana Cake Pop
app.MapHealthChecks("/health");

// ── Seed database ────────────────────────────────────────────────
await OrderDbContextSeed.SeedAsync(app.Services);

app.Run();
