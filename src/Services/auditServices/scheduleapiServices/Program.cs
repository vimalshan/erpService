using ScheduleService.Data;
using ScheduleService.Extensions;
using ScheduleService.GraphQL.Mutations;
using ScheduleService.GraphQL.Queries;
using ScheduleService.Middleware;
using ScheduleService.Repositories;
using ScheduleService.Services;
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
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IScheduleService, ScheduleService.Services.ScheduleService>();

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
app.MapGet("/api/schedules/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ScheduleService.Application.Queries.GetAllSchedulesQuery())))
    .WithTags("Schedules-Minimal");

app.MapGet("/api/schedules/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new ScheduleService.Application.Queries.GetScheduleByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Schedules-Minimal");

app.MapGet("/api/schedules/minimal/audit/{auditId}", async (int auditId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ScheduleService.Application.Queries.GetSchedulesByAuditQuery(auditId))))
    .WithTags("Schedules-Minimal");

app.MapGet("/api/schedules/minimal/site/{siteId}", async (int siteId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ScheduleService.Application.Queries.GetSchedulesBySiteQuery(siteId))))
    .WithTags("Schedules-Minimal");

app.MapPost("/api/schedules/minimal", async (ScheduleService.Application.DTOs.CreateAuditSiteAuditDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/schedules/minimal/{0}", await mediator.Send(new ScheduleService.Application.Commands.ScheduleAuditCommand(dto))))
    .WithTags("Schedules-Minimal");

app.MapPut("/api/schedules/minimal", async (ScheduleService.Application.DTOs.UpdateAuditSiteAuditDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ScheduleService.Application.Commands.UpdateScheduleCommand(dto))))
    .WithTags("Schedules-Minimal");

app.MapDelete("/api/schedules/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
    await mediator.Send(new ScheduleService.Application.Commands.DeleteScheduleCommand(id)) ? Results.NoContent() : Results.NotFound())
    .WithTags("Schedules-Minimal");

app.MapPut("/api/schedules/minimal/{id}/start", async (int id, DateTime? startDate, int? startedBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ScheduleService.Application.Commands.StartAuditCommand(id, startDate ?? DateTime.UtcNow, startedBy))))
    .WithTags("Schedules-Minimal");

app.MapPut("/api/schedules/minimal/{id}/complete", async (int id, DateTime? completionDate, string? reportPath, int? completedBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new ScheduleService.Application.Commands.CompleteAuditCommand(id, completionDate ?? DateTime.UtcNow, reportPath, completedBy))))
    .WithTags("Schedules-Minimal");

app.Run();
