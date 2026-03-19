using System.Text;
using MamAllocationService.Api.GraphQL;
using MamAllocationService.Api.Middleware;
using MamAllocationService.Api.MinimalApis;
using MamAllocationService.Application;
using MamAllocationService.Infrastructure;
using MamAllocationService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MAM Allocation Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(_ =>
    {
        var requirement = new OpenApiSecurityRequirement();
        var scheme = new OpenApiSecuritySchemeReference("Bearer");
        requirement.Add(scheme, new List<string>());
        return requirement;
    });
});

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MamAllocationServiceSuperSecretKey2026!@#$%^&*()";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MamAllocationService";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MamAllocationService";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<AllocationQuery>()
    .AddMutationType<AllocationMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddHealthChecks()
    .AddSqlServer(connStr, name: "database", tags: ["db", "sql"]);

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MAM Allocation Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();
app.MapHealthChecks("/health");

// Minimal APIs
app.MapAllocationMinimalApis();
app.MapArrivalMinimalApis();
app.MapConsumptionMinimalApis();
app.MapDispatchMinimalApis();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MamAllocationDbContext>();
    await SeedData.SeedAsync(context);
}

app.Run();
