using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaxService.Application.Extensions;
using TaxService.Infrastructure.Extensions;
using TaxService.Infrastructure.Data;
using TaxService.Infrastructure.MessageBroker;
using TaxService.API.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"TaxService\";Command Timeout=0";

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(connectionString, builder.Configuration);

// Add Authentication
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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TaxQuery>();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Map health check endpoint
app.MapHealthChecks("/health");

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

// Seed database with initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaxServiceDbContext>();
    await TaxServiceDbContextSeed.SeedAsync(dbContext);
}

// Connect to RabbitMQ (non-fatal if unavailable)
var rabbitMQ = app.Services.GetService<IMessageBrokerConnection>();
if (rabbitMQ != null)
    await rabbitMQ.ConnectAsync();

// Expose RabbitMQ status as a minimal endpoint
app.MapGet("/health/rabbitmq", (IMessageBrokerConnection? broker) =>
    Results.Ok(new
    {
        status = broker?.IsConnected == true ? "Connected" : "Disconnected",
        broker = "RabbitMQ",
        host = builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        port = builder.Configuration["RabbitMQ:Port"] ?? "5672"
    }));

app.Run();
