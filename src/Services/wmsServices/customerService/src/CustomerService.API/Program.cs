using System.Text;
using CustomerService.API.GraphQL;
using CustomerService.API.Middleware;
using CustomerService.Application;
using CustomerService.Infrastructure;
using CustomerService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ── Layer DI ────────────────────────────────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ───────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Customer Service API",
        Version = "v1",
        Description = "WMS Customer Microservice"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>()
    });
});

// ── JWT Authentication ──────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSuperSecretKeyThatIsAtLeast32BytesLong!!");

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
        ValidIssuer = jwtSettings["Issuer"] ?? "CustomerService",
        ValidAudience = jwtSettings["Audience"] ?? "CustomerService",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// ── GraphQL (Hot Chocolate) ─────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CustomerQuery>()
    .AddMutationType<CustomerMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// ── Health Checks ───────────────────────────────────────────────────────────
builder.Services
    .AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "database");

// ── Polly Circuit Breaker via HttpClient ────────────────────────────────────
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(CustomerService.API.Policies.PollyPolicies.GetCircuitBreakerPolicy())
    .AddPolicyHandler(CustomerService.API.Policies.PollyPolicies.GetRetryPolicy());

var app = builder.Build();

// ── Middleware ───────────────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ── Swagger ─────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ── Map endpoints ───────────────────────────────────────────────────────────
app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// ── Minimal APIs ────────────────────────────────────────────────────────────
CustomerService.API.MinimalApis.CustomerEndpoints.MapCustomerEndpoints(app);

// ── Apply migrations on startup (development only) ──────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
