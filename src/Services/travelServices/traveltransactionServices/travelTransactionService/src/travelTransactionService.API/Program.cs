using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using travelTransactionService.Application;
using travelTransactionService.Infrastructure;
using travelTransactionService.Infrastructure.Data;
using travelTransactionService.Infrastructure.Data.Seed;
using travelTransactionService.API.GraphQL;
using travelTransactionService.API.GraphQL.Types;
using travelTransactionService.API.Middleware;
using travelTransactionService.API.MinimalApis;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure DI
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
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

// GraphQL (HotChocolate / Banana Cake Pop)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<TransactionQuery>()
    .AddMutationType<TransactionMutation>()
    .AddType<VendorMasterType>()
    .AddType<TaxMasterType>()
    .AddType<JaiInterfaceLineType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("TravelDb")!,
        name: "sqlserver",
        tags: ["db", "sql"]);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel Transaction Service v1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection(); // Disabled: running HTTP-only profile
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapTransactionEndpoints();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await TransactionDbSeeder.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Database seeding failed.");
    }
}

app.Run();

