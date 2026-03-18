using CommunityService.API.Extensions;
using CommunityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddAuthenticationAndAuthorization(builder.Configuration)
    .AddApiServices();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CommunityService.API.GraphQL.Query>();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Build app
var app = builder.Build();

// Middleware pipeline
app.UseCustomMiddleware();
app.UseSwaggerAndHealthChecks();

// Only redirect to HTTPS in Production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapGraphQL("/graphql");

// EF Core migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CommunityDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
