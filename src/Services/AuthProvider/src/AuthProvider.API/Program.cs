using AuthProvider.Application;
using AuthProvider.API.GraphQL.Mutations;
using AuthProvider.API.GraphQL.Queries;
using AuthProvider.API.GraphQL.Subscriptions;
using AuthProvider.API.Middleware;
using AuthProvider.Infrastructure;
using Asp.Versioning;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;
using AuthProvider.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerUI;

// ── Bootstrap Serilog ─────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ServiceName", "AuthProvider")
    .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/auth-provider-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting AuthProvider API");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ───────────────────────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, services, cfg) => cfg
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ServiceName", "AuthProvider")
        .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(
            path: "logs/auth-provider-.txt",
            rollingInterval: RollingInterval.Day));

    // ── Application + Infrastructure DI ──────────────────────────────────────
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // ── Controllers + JSON ───────────────────────────────────────────────────
    builder.Services.AddControllers()
        .AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    // ── API Versioning ────────────────────────────────────────────────────────
    builder.Services.AddApiVersioning(o =>
    {
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.ReportApiVersions = true;
        o.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddApiExplorer(o =>
    {
        o.GroupNameFormat = "'v'VVV";
        o.SubstituteApiVersionInUrl = true;
    });

    // ── Swagger / OpenAPI ─────────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(o =>
    {
        // JWT bearer auth in Swagger UI
        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token: Bearer {token}"
        });
        o.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        
        // IncludeXmlComments for documentation, if exists
        var xmlFile = System.IO.Path.Combine(AppContext.BaseDirectory, "AuthProvider.API.xml");
        if (System.IO.File.Exists(xmlFile))
        {
            o.IncludeXmlComments(xmlFile);
        }
        
        // Add Swagger docs for default versions
        o.SwaggerDoc("v1", new OpenApiInfo 
        { 
            Title = "AuthProvider API", 
            Version = "v1.0",
            Description = "Authentication & Authorization microservice",
            Contact = new OpenApiContact { Name = "API Support", Email = "support@authprovider.local" }
        });
        o.SwaggerDoc("v2", new OpenApiInfo 
        { 
            Title = "AuthProvider API", 
            Version = "v2.0",
            Description = "Authentication & Authorization microservice - Enhanced",
            Contact = new OpenApiContact { Name = "API Support", Email = "support@authprovider.local" }
        });
    });

    // ── CORS ──────────────────────────────────────────────────────────────────
    builder.Services.AddCors(o => o.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    // ── HttpContextAccessor (used by GraphQL mutations) ───────────────────────
    builder.Services.AddHttpContextAccessor();

    // ── GraphQL (HotChocolate) ────────────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<UserQueryType>()
        .AddMutationType<AuthMutationType>()
        .AddSubscriptionType<AuthSubscriptionType>()
        .AddAuthorization()
        .AddInMemorySubscriptions()
        .ModifyRequestOptions(o =>
        {
            o.IncludeExceptionDetails = builder.Environment.IsDevelopment();
        });

    // ── Health checks ─────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks();

    // ─────────────────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ── Run EF Migrations on startup ──────────────────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            await db.Database.MigrateAsync();
            Log.Information("Database migration applied");
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Database migration failed – continuing (DB may not be available yet)");
        }
    }

    // ── Middleware pipeline ───────────────────────────────────────────────────
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseCors("AllowAll");

    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthProvider API v1.0");
        o.SwaggerEndpoint("/swagger/v2/swagger.json", "AuthProvider API v2.0");
        o.RoutePrefix = "swagger";
        o.DefaultModelsExpandDepth(2);
        o.DisplayOperationId();
    });

    app.UseWebSockets();            // required for GraphQL subscriptions
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // ── GraphQL endpoint ──────────────────────────────────────────────────────
    app.MapGraphQL("/graphql");

    // ── Minimal API endpoints ─────────────────────────────────────────────────
    var v1 = app.NewVersionedApi();

    v1.MapGet("/api/v{version:apiVersion}/minimal/auth/health", () =>
        Results.Ok(new { Status = "Healthy", Service = "AuthProvider", Timestamp = DateTime.UtcNow }))
        .HasApiVersion(1, 0)
        .HasApiVersion(2, 0)
        .WithName("HealthCheck")
        .WithTags("Health")
        .AllowAnonymous();

    v1.MapGet("/api/v{version:apiVersion}/minimal/auth/version", () =>
        Results.Ok(new { Version = "1.0", Framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription }))
        .HasApiVersion(1, 0)
        .HasApiVersion(2, 0)
        .WithName("VersionInfo")
        .WithTags("Health")
        .AllowAnonymous();

    // ── Health check endpoint ─────────────────────────────────────────────────
    app.MapHealthChecks("/health");

    Log.Information("AuthProvider API ready → http://localhost:5200 | Swagger → http://localhost:5200/swagger | GraphQL → http://localhost:5200/graphql");

    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "AuthProvider API terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

