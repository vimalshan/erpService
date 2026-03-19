using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using VehicleTracking.API.GraphQL;
using VehicleTracking.API.Middleware;
using VehicleTracking.Application;
using VehicleTracking.Infrastructure;
using VehicleTracking.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "VehicleTrackingSuperSecretKey2026!@#$%^&*()_+=";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "VehicleTrackingAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "VehicleTrackingClient";

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// GraphQL (Hot Chocolate / Banana Cake Pop)
builder.Services
    .AddGraphQLServer()
    .AddType<CharType>()
    .BindRuntimeType<char, CharType>()
    .AddQueryType<VehicleQuery>()
    .AddMutationType<VehicleMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Circuit Breaker with Polly
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)))
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<VehicleTracking.Infrastructure.Persistence.VehicleTrackingDbContext>("database");

var app = builder.Build();

// Seed database
await DatabaseSeeder.SeedAsync(app.Services);

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers (REST API)
app.MapControllers();

// Map GraphQL endpoint
app.MapGraphQL();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// === Minimal APIs ===
app.MapMinimalApis();

app.Run();

// Minimal API extension
public static class MinimalApiExtensions
{
    public static void MapMinimalApis(this WebApplication app)
    {
        var api = app.MapGroup("/api/minimal").RequireAuthorization();

        api.MapGet("/vehicles", async (MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetAllVehiclesQuery());
            return Results.Ok(result);
        }).WithName("GetAllVehiclesMinimal");

        api.MapGet("/vehicles/{id:long}", async (long id, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetVehicleByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetVehicleByIdMinimal");

        api.MapGet("/vehicles/{trackingNumber:long}/stages", async (long trackingNumber, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetVehicleStagesQuery(trackingNumber));
            return Results.Ok(result);
        }).WithName("GetVehicleStagesMinimal");

        api.MapGet("/transactions/active", async (MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetActiveTransactionsQuery());
            return Results.Ok(result);
        }).WithName("GetActiveTransactionsMinimal");

        api.MapGet("/stages", async (MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetAllStagesQuery());
            return Results.Ok(result);
        }).WithName("GetAllStagesMinimal");

        api.MapGet("/purposes", async (MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetAllPurposesQuery());
            return Results.Ok(result);
        }).WithName("GetAllPurposesMinimal");

        api.MapGet("/weight/{trackingNumber:long}", async (long trackingNumber, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(new VehicleTracking.Application.Vehicles.Queries.GetWeightInfoQuery(trackingNumber));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetWeightInfoMinimal");

        // === POST Minimal APIs ===
        api.MapPost("/vehicles", async (VehicleTracking.Application.Vehicles.Commands.RegisterVehicleCommand command, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/vehicles/{result.SerialNumber}", result);
        }).WithName("RegisterVehicleMinimal");

        api.MapPut("/vehicles/{id:long}", async (long id, VehicleTracking.Application.Vehicles.Commands.UpdateVehicleMasterCommand command, MediatR.IMediator mediator) =>
        {
            if (id != command.SerialNumber) return Results.BadRequest("Route id does not match body.");
            return Results.Ok(await mediator.Send(command));
        }).WithName("UpdateVehicleMinimal");

        api.MapPost("/transactions", async (VehicleTracking.Application.Vehicles.Commands.CreateVehicleTransactionCommand command, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("CreateTransactionMinimal");

        api.MapPost("/transactions/{trackingNumber:long}/close", async (long trackingNumber, VehicleTracking.Application.Vehicles.Commands.CloseVehicleTransactionCommand command, MediatR.IMediator mediator) =>
        {
            if (trackingNumber != command.TrackingNumber) return Results.BadRequest("Route trackingNumber does not match body.");
            return Results.Ok(await mediator.Send(command));
        }).WithName("CloseTransactionMinimal");

        api.MapPost("/invoices", async (VehicleTracking.Application.Vehicles.Commands.CreateVehicleInvoiceCommand command, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("CreateInvoiceMinimal");

        api.MapPost("/decisions", async (VehicleTracking.Application.Vehicles.Commands.MakeDecisionCommand command, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("MakeDecisionMinimal");

        api.MapPost("/weight", async (VehicleTracking.Application.Vehicles.Commands.UpdateWeightInfoCommand command, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("UpdateWeightMinimal");

        api.MapPost("/stages", async (VehicleTracking.Application.Vehicles.Commands.UpdateVehicleStageCommand command, MediatR.IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("UpdateStageMinimal");
    }
}
