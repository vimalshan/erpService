using DemandManagement.Application;
using DemandManagement.Infrastructure;
using DemandManagement.API.GraphQL;
using DemandManagement.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DemandManagement.Application.Commands;
using DemandManagement.Application.DTOs;
using MediatR;
using DemandManagement.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// GraphQL
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapGraphQL("/graphql");

// Minimal APIs
app.MapPost("/api/demand", async (CreateDemandRequest request, IMediator mediator) =>
{
    var command = new CreateDemandCommand(request);
    var result = await mediator.Send(command);
    return Results.Created($"/api/demand/{result}", result);
})
.WithName("CreateDemand")
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/api/demand/{id}", async (long id, IDemandRepository repository) =>
{
    var demand = await repository.GetByIdAsync(id);
    return demand != null ? Results.Ok(demand) : Results.NotFound();
})
.WithName("GetDemandById")
.WithOpenApi();

app.Run();
