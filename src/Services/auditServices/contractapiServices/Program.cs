using ContractService.Data;
using ContractService.Extensions;
using ContractService.GraphQL.Mutations;
using ContractService.GraphQL.Queries;
using ContractService.Middleware;
using ContractService.Repositories;
using ContractService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractService, ContractService.Services.ContractService>();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddMessagingServices(builder.Configuration);
builder.Services.AddHealthCheckServices(builder.Configuration);

var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32Characters!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("role", "admin"));
    options.AddPolicy("Auditor", policy => policy.RequireClaim("role", "auditor", "admin"));
    options.AddPolicy("User", policy => policy.RequireClaim("role", "user", "auditor", "admin"));
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering().AddSorting().AddProjections()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// Minimal API endpoints
app.MapGet("/api/contracts/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ContractService.Application.Queries.GetAllContractsQuery())))
    .WithTags("Contracts-Minimal");

app.MapGet("/api/contracts/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new ContractService.Application.Queries.GetContractByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Contracts-Minimal");

app.MapGet("/api/contracts/minimal/company/{companyId}", async (int companyId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ContractService.Application.Queries.GetContractsByCompanyQuery(companyId))))
    .WithTags("Contracts-Minimal");

app.MapPost("/api/contracts/minimal", async (ContractService.Application.DTOs.CreateContractDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/contracts/minimal/{0}", await mediator.Send(new ContractService.Application.Commands.CreateContractCommand(dto))))
    .WithTags("Contracts-Minimal");

app.MapPut("/api/contracts/minimal", async (ContractService.Application.DTOs.UpdateContractDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ContractService.Application.Commands.UpdateContractCommand(dto))))
    .WithTags("Contracts-Minimal");

app.MapDelete("/api/contracts/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
    await mediator.Send(new ContractService.Application.Commands.DeleteContractCommand(id)) ? Results.NoContent() : Results.NotFound())
    .WithTags("Contracts-Minimal");

app.MapPut("/api/contracts/minimal/{id}/status", async (int id, string newStatus, int? modifiedBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ContractService.Application.Commands.ChangeContractStatusCommand(id, newStatus, modifiedBy))))
    .WithTags("Contracts-Minimal");

app.MapPost("/api/contracts/minimal/{id}/renew", async (int id, DateTime? newEndDate, int? modifiedBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ContractService.Application.Commands.RenewContractCommand(id, newEndDate, modifiedBy))))
    .WithTags("Contracts-Minimal");

app.Run();
