using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PFTransactionalService.API.GraphQL;
using PFTransactionalService.API.Middleware;
using PFTransactionalService.API.MinimalApis;
using PFTransactionalService.Application;
using PFTransactionalService.Infrastructure;
using PFTransactionalService.Infrastructure.Persistence;
using PFTransactionalService.Infrastructure.Persistence.EfCore;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSuperSecretKeyThatIsAtLeast32Chars!");

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
        ValidIssuer = jwtSettings["Issuer"] ?? "PFTransactionalService",
        ValidAudience = jwtSettings["Audience"] ?? "PFTransactionalServiceClients",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PF Transactional Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(doc =>
    {
        var requirement = new OpenApiSecurityRequirement();
        requirement[new OpenApiSecuritySchemeReference("Bearer", doc, null!)] = new List<string>();
        return requirement;
    });
});

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<PFAccumulationType>()
    .AddAuthorization();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("PFTransactionalDb")!,
        name: "sqlserver",
        tags: ["db", "sql"])
    .AddCheck("rabbitmq", new PFTransactionalService.API.Health.RabbitMqHealthCheck(
        builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
        int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672"),
        builder.Configuration["RabbitMQ:UserName"] ?? "guest",
        builder.Configuration["RabbitMQ:Password"] ?? "guest"),
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
        tags: ["messaging"]);

// Polly Circuit Breaker (for HttpClient via Microsoft.Extensions.Http.Resilience)
builder.Services.AddHttpClient("ExternalService")
    .AddStandardResilienceHandler();

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PF Transactional Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapGraphQL();
app.MapPFTransactionEndpoints();
app.MapHealthChecks("/health");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PFTransactionalDbContext>();
    await SeedData.SeedAsync(context);
}

app.Run();
