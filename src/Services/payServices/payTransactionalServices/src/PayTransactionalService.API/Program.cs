using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PayTransactionalService.Application.Extensions;
using PayTransactionalService.Infrastructure.Extensions;
using PayTransactionalService.Infrastructure.Persistence;
using PayTransactionalService.Infrastructure.MessageBroker;
using PayTransactionalService.Infrastructure.SeedData;
using PayTransactionalService.API.GraphQL;
using PayTransactionalService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");

// Register services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(connectionString, builder.Configuration);

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "your-secret-key-change-in-production");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Health Checks
builder.Services.AddHealthChecks();

// Swagger
builder.Services.AddSwaggerGen();

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<PayTransactionalQuery>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health checks
app.MapHealthChecks("/health");

// GraphQL
app.MapGraphQL("/graphql");

// Minimal API endpoints
app.MapGet("/health/rabbitmq", (IMessageBrokerConnection? broker) =>
    Results.Ok(new
    {
        status = broker?.IsConnected == true ? "Connected" : "Disconnected",
        broker = "RabbitMQ",
        host = builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        port = builder.Configuration["RabbitMQ:Port"] ?? "5672"
    }));

app.MapGet("/api/summary/month/{monthYear}", async (string monthYear, MediatR.IMediator mediator) =>
{
    var txns = await mediator.Send(new PayTransactionalService.Application.Queries.GetPayTransactionsByMonthQuery(monthYear));
    if (!txns.IsSuccess) return Results.NotFound(new { message = txns.Error });
    var data = txns.Data!.ToList();
    return Results.Ok(new
    {
        monthYear,
        totalTransactions = data.Count,
        totalGross = data.Sum(t => t.GrossAmount),
        totalDeductions = data.Sum(t => t.Deductions),
        totalNet = data.Sum(t => t.NetAmount)
    });
});

// Seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PayTransactionalDbContext>();
    await PayTransactionalDbContextSeed.SeedAsync(dbContext);
}

// Connect RabbitMQ (non-fatal)
var rabbitMQ = app.Services.GetService<IMessageBrokerConnection>();
if (rabbitMQ != null) await rabbitMQ.ConnectAsync();

app.Run();
