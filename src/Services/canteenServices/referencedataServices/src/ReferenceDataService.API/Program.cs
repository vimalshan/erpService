using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ReferenceDataService.API.GraphQL;
using ReferenceDataService.API.GraphQL.Types;
using ReferenceDataService.API.Middleware;
using ReferenceDataService.API.MinimalApis;
using ReferenceDataService.Application;
using ReferenceDataService.Infrastructure;
using ReferenceDataService.Infrastructure.Persistence;
using ReferenceDataService.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ReferenceData Service API",
        Version = "v1",
        Description = "Canteen Reference Data Microservice"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<LovMasterType>()
    .AddType<LovTypeMasterType>()
    .AddType<PathToSqlServerType>()
    .AddFiltering()
    .AddSorting();

// Health Checks
builder.Services.AddHealthChecks();

// Polly Circuit Breaker is configured on HttpClient policies in Infrastructure

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Swagger (enabled in all environments for internal microservice)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReferenceData Service API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapReferenceDataEndpoints();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReferenceDataDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await ReferenceDataSeeder.SeedAsync(context, logger);
}

app.Run();

public partial class Program { }

