using NotificationService.Data;
using NotificationService.Extensions;
using NotificationService.GraphQL.Mutations;
using NotificationService.GraphQL.Queries;
using NotificationService.Middleware;
using NotificationService.Repositories;
using NotificationService.Services;
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
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService.Services.NotificationService>();

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
app.MapGet("/api/notifications/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new NotificationService.Application.Queries.GetAllNotificationsQuery())))
    .WithTags("Notifications-Minimal");

app.MapGet("/api/notifications/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new NotificationService.Application.Queries.GetNotificationByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Notifications-Minimal");

app.MapPost("/api/notifications/minimal", async (NotificationService.Application.DTOs.CreateNotificationDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/notifications/minimal/{0}", await mediator.Send(new NotificationService.Application.Commands.CreateNotificationCommand(dto))))
    .WithTags("Notifications-Minimal");

app.MapPut("/api/notifications/minimal", async (NotificationService.Application.DTOs.UpdateNotificationDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new NotificationService.Application.Commands.UpdateNotificationCommand(dto))))
    .WithTags("Notifications-Minimal");

app.MapDelete("/api/notifications/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
    await mediator.Send(new NotificationService.Application.Commands.DeleteNotificationCommand(id)) ? Results.NoContent() : Results.NotFound())
    .WithTags("Notifications-Minimal");

app.MapPut("/api/notifications/minimal/{id}/read", async (int id, int userId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new NotificationService.Application.Commands.MarkNotificationReadCommand(id, userId))))
    .WithTags("Notifications-Minimal");

app.MapPut("/api/notifications/minimal/{id}/archive", async (int id, int? modifiedBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new NotificationService.Application.Commands.ArchiveNotificationCommand(id, modifiedBy))))
    .WithTags("Notifications-Minimal");

app.MapGet("/api/notifications/minimal/categories", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new NotificationService.Application.Queries.GetNotificationCategoriesQuery())))
    .WithTags("Notifications-Minimal");

app.MapPost("/api/notifications/minimal/categories", async (NotificationService.Application.DTOs.CreateNotificationCategoryDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/notifications/minimal/categories/{0}", await mediator.Send(new NotificationService.Application.Commands.CreateNotificationCategoryCommand(dto))))
    .WithTags("Notifications-Minimal");

app.Run();
