using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RiskService.API.Extensions;
using RiskService.API.GraphQL.Mutations;
using RiskService.API.GraphQL.Queries;
using RiskService.API.GraphQL.Types;
using RiskService.API.Middleware;
using RiskService.API.MinimalApis;
using RiskService.Application;
using RiskService.Infrastructure;
using RiskService.Infrastructure.Persistence;
using RiskService.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication & Authorization (JWT)
var jwtSettings = builder.Configuration.GetSection("Jwt");
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});
builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Risk Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", null),
            new List<string>()
        }
    });
});
// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<RiskQuery>()
    .AddMutationType<RiskMutation>()
    .AddType<RiskGraphType>()
    .AddType<RiskCauseGraphType>()
    .AddType<RiskControlGraphType>()
    .AddType<MitigationGraphType>()
    .AddType<MitigationActionGraphType>()
    .AddType<SelfAssessmentGraphType>()
    .AddType<RiskTypeGraphType>();

// Polly Circuit Breaker
builder.Services.AddPollyPolicies();

// Health Checks
builder.Services.AddRiskHealthChecks(builder.Configuration);

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Risk Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapGraphQL();
app.MapRiskMinimalApis();
app.MapHealthChecks("/health");

// Database migration and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RiskDbContext>();
    await db.Database.MigrateAsync();
    await RiskDbSeeder.SeedAsync(db);
}

app.Run();

