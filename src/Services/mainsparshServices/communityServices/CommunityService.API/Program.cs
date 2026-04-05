using CommunityService.API.Extensions;
using CommunityService.Infrastructure.Persistence;
using CommunityService.Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MediatR;
using CommunityService.Application.Queries;
using CommunityService.Application.Commands;
using CommunityService.Application.DTOs;

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
    .AddQueryType<CommunityService.API.GraphQL.Query>()
    .AddMutationType<CommunityService.API.GraphQL.Mutation>();

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

// Auth token endpoint (dev/test only)
app.MapPost("/api/auth/token", (TokenRequest req, IConfiguration cfg) =>
{
    var jwt = cfg.GetSection("Jwt");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SigningKey"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, req.Username),
        new Claim(ClaimTypes.Role, "User"),
        new Claim("sub", req.Username)
    };
    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds);
    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
}).AllowAnonymous();

// Minimal API group v2
var communitiesV2 = app.MapGroup("/api/v2/communities").RequireAuthorization();

communitiesV2.MapGet("/", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetAllCommunitiesQuery(1, 10));
    return Results.Ok(result);
});

communitiesV2.MapGet("/{id:long}", async (IMediator mediator, long id) =>
{
    var result = await mediator.Send(new GetCommunityByIdQuery(id));
    return result is null ? Results.NotFound() : Results.Ok(result);
});

communitiesV2.MapPost("/", async (IMediator mediator, CreateCommunityDto dto) =>
{
    var result = await mediator.Send(new CreateCommunityCommand(dto));
    return Results.Created($"/api/v2/communities/{result.CommunityId}", result);
});

// RabbitMQ test endpoint
app.MapGet("/api/rabbitmq/test", (HttpContext ctx) =>
{
    try
    {
        var publisher = ctx.RequestServices.GetRequiredService<IMessagePublisher>();
        return Results.Ok(new { status = "Connected", message = "RabbitMQ publisher resolved successfully" });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { status = "Unavailable", message = ex.Message });
    }
}).AllowAnonymous();

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

record TokenRequest(string Username, string Password);
