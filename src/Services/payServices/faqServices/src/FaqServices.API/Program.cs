using FaqServices.API.Endpoints;
using FaqServices.Application.Extensions;
using FaqServices.Infrastructure.Extensions;
using FaqServices.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/faq-api-.txt", rollingInterval: RollingInterval.Day));

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Feature Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));

// Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey") ?? throw new InvalidOperationException("JWT SecretKey not configured");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidateAudience = true,
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Health Checks
builder.Services
    .AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string not found"))
    .AddCheck("API Health", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

// GraphQL Configuration
builder.Services
    .AddGraphQLServer()
    .AddQueryType<FaqServices.API.GraphQL.Queries.FaqQuery>()
    .AddMutationType<FaqServices.API.GraphQL.Mutations.FaqMutation>()
    .AddSubscriptionType<FaqServices.API.GraphQL.Subscriptions.FaqSubscription>()
    .AddType<FaqServices.API.GraphQL.Types.FaqGradeType>()
    .AddType<FaqServices.API.GraphQL.Types.FaqQuestionType>()
    .AddType<FaqServices.API.GraphQL.Types.FaqAnswerType>();

var app = builder.Build();

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FaqServices.Infrastructure.Data.FaqDbContext>();
    await DatabaseInitializer.InitializeAsync(context);
}

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint
app.MapHealthChecks("/health");

// GraphQL endpoint
app.MapGraphQL("/graphql");

// API endpoints
app.MapControllers();

// Minimal APIs
app.MapGroup("/api/grades")
    .WithName("Grades")
    .WithOpenApi()
    .MapGradeEndpoints();

app.MapGroup("/api/questions")
    .WithName("Questions")
    .WithOpenApi()
    .MapQuestionEndpoints();

app.MapGroup("/api/answers")
    .WithName("Answers")
    .WithOpenApi()
    .MapAnswerEndpoints();

app.Run();
