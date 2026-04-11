using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TransactionService.API.GraphQL;
using TransactionService.API.Middleware;
using TransactionService.API.MinimalApis;
using TransactionService.Application;
using TransactionService.Infrastructure;
using TransactionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transaction Services API",
        Version = "v1",
        Description = "Financial Transaction Microservice — JV, Batches, Payments & Invoices"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
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

// ── JWT Authentication ───────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");

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

// ── Application & Infrastructure ─────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── GraphQL ──────────────────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddErrorFilter<GraphQLErrorFilter>();

// ── Health Checks ─────────────────────────────────────────────────────────────
var healthChecks = builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("TransactionDb")!,
        name: "transactiondb-sql",
        tags: ["db", "sql"]);

if (builder.Configuration.GetValue<bool>("RabbitMQ:Enabled", false))
{
    healthChecks.AddRabbitMQ(
        sp => sp.GetRequiredService<RabbitMQ.Client.IConnection>(),
        name: "rabbitmq",
        tags: ["messaging"]);
}

// ── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// Verify DB connectivity on startup (tables managed via SQL scripts, not EF migrations)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    if (await db.Database.CanConnectAsync())
    {
        logger.LogInformation("Database connection verified successfully.");
        await TransactionService.Infrastructure.Persistence.Seed.TransactionDbContextSeed.SeedAsync(db, logger);
    }
    else
    {
        logger.LogWarning("Cannot connect to database. Seeding skipped.");
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Services API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapTransactionEndpoints();

// GraphQL endpoint - Banana Cake Pop UI at /graphql
app.MapGraphQL("/graphql");

// Health check endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

app.Run();
