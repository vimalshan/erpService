using System.Text;
using MeetingModule.API.GraphQL;
using MeetingModule.API.Middleware;
using MeetingModule.API.MinimalApis;
using MeetingModule.Application;
using MeetingModule.Infrastructure;
using MeetingModule.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Application & Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "MeetingModule API",
        Version = "v1",
        Description = "Meeting Type and Schedule Management Microservice"
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(doc =>
    {
        var requirement = new Microsoft.OpenApi.OpenApiSecurityRequirement();
        var scheme = new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer");
        requirement.Add(scheme, new List<string>());
        return requirement;
    });
});

// JWT Authentication
var jwtKey = configuration["Jwt:Key"]!;
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
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();

// GraphQL (Hot Chocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<MeetingQuery>()
    .AddMutationType<MeetingMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!, name: "database", tags: ["db", "sql"]);

// Polly Circuit Breaker via HttpClientFactory
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(MeetingModule.API.Middleware.PollyPolicies.GetRetryPolicy())
    .AddPolicyHandler(MeetingModule.API.Middleware.PollyPolicies.GetCircuitBreakerPolicy());

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MeetingDbContext>();
    await MeetingDbSeeder.SeedAsync(context);
}

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetingModule API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapGraphQL();
app.MapMeetingMinimalApis();
app.MapHealthChecks("/health");

app.Run();
