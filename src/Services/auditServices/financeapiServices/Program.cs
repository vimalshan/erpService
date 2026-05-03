using FinanceService.Data;
using FinanceService.Extensions;
using FinanceService.GraphQL.Mutations;
using FinanceService.GraphQL.Queries;
using FinanceService.Middleware;
using FinanceService.Repositories;
using FinanceService.Services;
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
builder.Services.AddScoped<IFinanceRepository, FinanceRepository>();
builder.Services.AddScoped<IFinanceService, FinanceService.Services.FinanceService>();

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
    .AddErrorFilter<FinanceService.GraphQL.GraphQLErrorFilter>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

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
app.MapGet("/api/invoices/minimal", async (MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new FinanceService.Application.Queries.GetAllInvoicesQuery())))
    .WithTags("Invoices-Minimal");

app.MapGet("/api/invoices/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new FinanceService.Application.Queries.GetInvoiceByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Invoices-Minimal");

app.MapGet("/api/invoices/minimal/company/{companyId}", async (int companyId, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new FinanceService.Application.Queries.GetInvoicesByCompanyQuery(companyId))))
    .WithTags("Invoices-Minimal");

app.MapPost("/api/invoices/minimal", async (FinanceService.Application.DTOs.CreateInvoiceDto dto, MediatR.IMediator mediator) =>
    Results.Created($"/api/invoices/minimal/{0}", await mediator.Send(new FinanceService.Application.Commands.CreateInvoiceCommand(dto))))
    .WithTags("Invoices-Minimal");

app.MapPut("/api/invoices/minimal", async (FinanceService.Application.DTOs.UpdateInvoiceDto dto, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new FinanceService.Application.Commands.UpdateInvoiceCommand(dto))))
    .WithTags("Invoices-Minimal");

app.MapDelete("/api/invoices/minimal/{id}", async (int id, MediatR.IMediator mediator) =>
    await mediator.Send(new FinanceService.Application.Commands.DeleteInvoiceCommand(id)) ? Results.NoContent() : Results.NotFound())
    .WithTags("Invoices-Minimal");

app.MapPut("/api/invoices/minimal/{id}/pay", async (int id, DateTime? paidDate, string? paymentMethod, string? paymentReference, int? paidBy, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new FinanceService.Application.Commands.MarkInvoicePaidCommand(id, paidDate ?? DateTime.UtcNow, paymentMethod, paymentReference, paidBy))))
    .WithTags("Invoices-Minimal");

app.MapGet("/api/financials/minimal/company/{companyId}", async (int companyId, int? year, MediatR.IMediator mediator) =>
    Results.Ok(await mediator.Send(new FinanceService.Application.Queries.GetFinancialsByCompanyQuery(companyId, year))))
    .WithTags("Financials-Minimal");

app.Run();
