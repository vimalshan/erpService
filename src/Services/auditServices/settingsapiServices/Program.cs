using SettingsService.Data;
using SettingsService.Extensions;
using SettingsService.GraphQL.Mutations;
using SettingsService.GraphQL.Queries;
using SettingsService.Middleware;
using SettingsService.Repositories;
using SettingsService.Services;
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
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ISettingsService, SettingsService.Services.SettingsService>();

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
app.MapGet("/api/users/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new SettingsService.Application.Queries.GetAllUsersQuery())))
    .WithTags("Users-Minimal");

app.MapGet("/api/users/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new SettingsService.Application.Queries.GetUserByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Users-Minimal");

app.MapPost("/api/users/minimal", async (SettingsService.Application.DTOs.CreateUserDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/users/minimal/{0}", await mediator.Send(new SettingsService.Application.Commands.CreateUserCommand(dto))))
    .WithTags("Users-Minimal");

app.MapPut("/api/users/minimal", async (SettingsService.Application.DTOs.UpdateUserDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new SettingsService.Application.Commands.UpdateUserCommand(dto))))
    .WithTags("Users-Minimal");

app.MapPut("/api/users/minimal/{id}/deactivate", async (int id, int? modifiedBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new SettingsService.Application.Commands.DeactivateUserCommand(id, modifiedBy))))
    .WithTags("Users-Minimal");

app.MapGet("/api/roles/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new SettingsService.Application.Queries.GetAllRolesQuery())))
    .WithTags("Roles-Minimal");

app.MapPost("/api/roles/minimal", async (SettingsService.Application.DTOs.CreateRoleDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/roles/minimal/{0}", await mediator.Send(new SettingsService.Application.Commands.CreateRoleCommand(dto))))
    .WithTags("Roles-Minimal");

app.MapGet("/api/users/minimal/{userId}/preferences", async (int userId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new SettingsService.Application.Queries.GetUserPreferencesQuery(userId))))
    .WithTags("Preferences-Minimal");

app.MapPost("/api/users/minimal/preferences", async (SettingsService.Application.DTOs.SetUserPreferenceDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new SettingsService.Application.Commands.SetUserPreferenceCommand(dto))))
    .WithTags("Preferences-Minimal");

app.Run();
