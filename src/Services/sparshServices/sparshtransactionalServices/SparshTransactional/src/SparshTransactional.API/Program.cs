using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HotChocolate.Types;
using Serilog;
using SparshTransactional.Application;
using SparshTransactional.Infrastructure;
using SparshTransactional.Infrastructure.Data;
using SparshTransactional.API.Endpoints;
using SparshTransactional.API.GraphQL;
using SparshTransactional.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Application & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(
    jwtSettings["SecretKey"] ?? "SparshTransactional_SuperSecret_Key_2026_Minimum32Chars!");

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
        ValidIssuer = jwtSettings["Issuer"] ?? "SparshTransactional.API",
        ValidAudience = jwtSettings["Audience"] ?? "SparshTransactional.Client",
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ScholarshipQuery>()
    .AddMutationType<ScholarshipMutation>()
    .TryAddTypeInterceptor<IgnoreDomainEventsTypeInterceptor>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .BindRuntimeType<char, StringType>()
    .BindRuntimeType<char?, StringType>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: ["db", "sql"]);

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapScholarshipEndpoints();
app.MapGraphQL();
app.MapHealthChecks("/health");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SparshTransactionalDbContext>();
    await SeedData.SeedAsync(context);
}

app.Run();
