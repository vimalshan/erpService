using System.Text;
using InvestmentService.Application;
using InvestmentService.Application.Commands;
using InvestmentService.Application.DTOs;
using InvestmentService.Application.Queries;
using InvestmentService.API.GraphQL;
using InvestmentService.API.Middleware;
using InvestmentService.Infrastructure;
using InvestmentService.Infrastructure.Data;
using InvestmentService.Infrastructure.Data.Seed;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Application & Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
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
builder.Services.AddSwaggerGen();

// GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("InvestmentDb")!,
        name: "database",
        tags: new[] { "db", "sql" });

// Polly Circuit Breaker is applied via HttpClient factory
builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddStandardResilienceHandler();

var app = builder.Build();

// Middleware
app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investment Service V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

// Minimal APIs
app.MapGet("/api/minimal/investments", async (IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetAllInvestmentsQuery())))
    .WithName("GetAllInvestmentsMinimal")
    .WithTags("Investments-Minimal")
    .RequireAuthorization();

app.MapGet("/api/minimal/investments/{invNo:long}", async (long invNo, IMediator mediator) =>
{
    var result = await mediator.Send(new GetInvestmentByIdQuery(invNo));
    return result == null ? Results.NotFound() : Results.Ok(result);
})
.WithName("GetInvestmentByIdMinimal")
.WithTags("Investments-Minimal")
.RequireAuthorization();

app.MapGet("/api/minimal/investments/active", async (IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetActiveInvestmentsQuery())))
    .WithName("GetActiveInvestmentsMinimal")
    .WithTags("Investments-Minimal")
    .RequireAuthorization();

app.MapGet("/api/minimal/portfolio-summary", async (IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetPortfolioSummaryQuery())))
    .WithName("GetPortfolioSummaryMinimal")
    .WithTags("Investments-Minimal")
    .RequireAuthorization();

app.MapPost("/api/minimal/investments", async (CreateInvestmentCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/api/minimal/investments/{result.InvNo}", result);
})
.WithName("CreateInvestmentMinimal")
.WithTags("Investments-Minimal")
.RequireAuthorization();

app.MapGet("/api/minimal/categories", async (IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetAllCategoriesQuery())))
    .WithName("GetAllCategoriesMinimal")
    .WithTags("MasterData-Minimal")
    .RequireAuthorization();

app.MapGet("/api/minimal/brokers", async (IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetAllBrokersQuery())))
    .WithName("GetAllBrokersMinimal")
    .WithTags("MasterData-Minimal")
    .RequireAuthorization();

// Seed database
using (var scope = app.Services.CreateScope())
{
    await InvestmentDbSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();
