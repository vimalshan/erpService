using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using LovService.API.Endpoints;
using LovService.API.GraphQL.Mutations;
using LovService.API.GraphQL.Queries;
using LovService.API.GraphQL.Subscriptions;
using LovService.API.Middleware;
using LovService.Application.Behaviors;
using LovService.Application.Validators;
using LovService.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

// ── Serilog Bootstrap ────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "LovService")
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog Full Configuration ────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, cfg) => cfg
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Service", "LovService")
        .WriteTo.Console()
        .WriteTo.File("logs/lovservice-.log", rollingInterval: RollingInterval.Day));

    // ── Application Insights ─────────────────────────────────────────────────
    var appInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
    {
        builder.Services.AddApplicationInsightsTelemetry();
    }

    // ── CORS ──────────────────────────────────────────────────────────────────
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("LovPolicy", policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

    // ── API Versioning ────────────────────────────────────────────────────────
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"),
            new QueryStringApiVersionReader("api-version"));
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    // ── JWT Authentication ────────────────────────────────────────────────────
    var jwtKey = builder.Configuration["Jwt:Key"] ?? "LovService-SuperSecretKey-DoNotExposeInProduction-2026";
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "LovService",
                ValidAudience = builder.Configuration["Jwt:Audience"] ?? "LovServiceClients",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("ReadOnly", policy => policy.RequireRole("Admin", "Reader"));
    });

    // ── MediatR + CQRS Behaviors ──────────────────────────────────────────────
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(LovService.Application.Behaviors.LoggingBehavior<,>).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    });

    // ── FluentValidation ──────────────────────────────────────────────────────
    builder.Services.AddValidatorsFromAssemblyContaining<CreateLovTypeCommandValidator>();

    // ── Infrastructure (EF, Dapper, RabbitMQ, Blob) ───────────────────────────
    builder.Services.AddInfrastructure(builder.Configuration);

    // ── Swagger ───────────────────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title       = "LOV Service API",
            Version     = "v1",
            Description = "List of Values Microservice — ERP Admin Services",
            Contact     = new OpenApiContact { Name = "Admin Team" }
        });

        // Suppress duplicate-route conflicts produced by versioned Minimal API groups
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header. Format: Bearer {token}",
            Name        = "Authorization",
            In          = ParameterLocation.Header,
            Type        = SecuritySchemeType.ApiKey,
            Scheme      = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                []
            }
        });
    });

    // ── GraphQL (HotChocolate) ────────────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<LovQuery>()
        .AddMutationType<LovMutation>()
        .AddSubscriptionType<LovSubscription>()
        .AddInMemorySubscriptions()
        .AddProjections()
        .AddFiltering()
        .AddSorting();

    // ── Build ─────────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ── Apply EF Core migrations on startup ───────────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<LovService.Infrastructure.Data.LovDbContext>();
        db.Database.Migrate();
    }

    // ── Middleware Pipeline ───────────────────────────────────────────────────
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    // Swagger must be before the error handler so exceptions surface naturally
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LOV Service API v1");
        c.RoutePrefix = "swagger";
    });

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseCors("LovPolicy");
    app.UseWebSockets();

    app.UseAuthentication();
    app.UseAuthorization();

    // ── GraphQL endpoint ──────────────────────────────────────────────────────
    app.MapGraphQL("/graphql");

    // ── Minimal API Endpoints (versioned) ────────────────────────────────────
    var v1 = app.NewVersionedApi().MapGroup("/api/v{version:apiVersion}").HasApiVersion(1, 0);

    v1.MapGroup("/lov-types").MapLovTypeEndpoints().WithTags("LOV Types");
    v1.MapGroup("/lov-masters").MapLovMasterEndpoints().WithTags("LOV Masters");
    v1.MapGroup("/item-data").MapItemDataEndpoints().WithTags("Item Data");

    // ── Health Check ──────────────────────────────────────────────────────────
    app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "LovService", timestamp = DateTime.UtcNow }))
       .WithTags("Health").AllowAnonymous();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "LovService failed to start");
}
finally
{
    Log.CloseAndFlush();
}
