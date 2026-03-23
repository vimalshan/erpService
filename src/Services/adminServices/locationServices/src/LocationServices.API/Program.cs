using Asp.Versioning;
using LocationServices.API.GraphQL.Mutations;
using LocationServices.API.GraphQL.Queries;
using LocationServices.API.GraphQL.Subscriptions;
using LocationServices.API.GraphQL.Types;
using LocationServices.API.Middleware;
using LocationServices.API.Services.AuthProvider;
using LocationServices.Application;
using LocationServices.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

// ═══════════════════════════════════════════════════════════════════
// SERILOG — bootstrap logger
// ═══════════════════════════════════════════════════════════════════
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/location-services-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ──────────────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("logs/location-services-.log", rollingInterval: RollingInterval.Day));

    // ── Application Insights ─────────────────────────────────────────
    builder.Services.AddApplicationInsightsTelemetry();

    // ── CORS ─────────────────────────────────────────────────────────
    builder.Services.AddCors(opt =>
        opt.AddPolicy("AllowAll", p =>
            p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    // ── JWT Authentication ────────────────────────────────────────────
    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
    builder.Services.AddScoped<IJwtService, JwtService>();

    var jwtSecret = builder.Configuration["Jwt:Secret"]!;
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer           = true,
                ValidIssuer              = builder.Configuration["Jwt:Issuer"],
                ValidateAudience         = true,
                ValidAudience            = builder.Configuration["Jwt:Audience"],
                ValidateLifetime         = true,
                ClockSkew                = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization(opt =>
    {
        opt.AddPolicy("AdminOnly",   p => p.RequireRole("Admin"));
        opt.AddPolicy("ManagerPlus", p => p.RequireRole("Admin", "LocationManager"));
    });

    // ── API Versioning ────────────────────────────────────────────────
    builder.Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion                  = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions                  = true;
        opt.ApiVersionReader                   = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"),
            new QueryStringApiVersionReader("api-version"));
    })
    .AddApiExplorer(opt =>
    {
        opt.GroupNameFormat           = "'v'VVV";
        opt.SubstituteApiVersionInUrl = true;
    });

    // ── Controllers ───────────────────────────────────────────────────
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // ── Swagger ───────────────────────────────────────────────────────
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Location Services API", Version = "v1",
            Description = "ERP Location-App Mapping Microservice — REST API v1" });
        opt.SwaggerDoc("v2", new OpenApiInfo { Title = "Location Services API", Version = "v2",
            Description = "Location-App Mapping API v2 with pagination" });

        var secScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization", Type = SecuritySchemeType.Http,
            Scheme = "bearer", BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token (without 'Bearer ' prefix)"
        };
        opt.AddSecurityDefinition("Bearer", secScheme);
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                    { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                Array.Empty<string>()
            }
        });
    });

    // ── Application + Infrastructure DI ──────────────────────────────
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // ── GraphQL (HotChocolate) ────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<LocationQuery>()
        .AddMutationType<LocationMutation>()
        .AddSubscriptionType<LocationSubscription>()
        .AddType<LocationAppMapType>()
        .AddInMemorySubscriptions()
        .AddFiltering()
        .AddSorting()
        .AddProjections()
        .AddMutationConventions()
        .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

    // ── Health Checks ─────────────────────────────────────────────────
    builder.Services.AddHealthChecks();

    // ═══════════════════════════════════════════════════════════════════
    // PIPELINE
    // ═══════════════════════════════════════════════════════════════════
    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSerilogRequestLogging();
    app.UseCors("AllowAll");

    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Location Services API v1");
        opt.SwaggerEndpoint("/swagger/v2/swagger.json", "Location Services API v2");
        opt.RoutePrefix = "swagger";
    });

    app.UseWebSockets();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapGraphQL();   // Banana Cake Pop → http://localhost:7136/graphql
    app.MapHealthChecks("/health");

    // ── Minimal API ───────────────────────────────────────────────────
    app.MapGet("/api/minimal/ping",
        () => Results.Ok(new { status = "ok", ts = DateTime.UtcNow }))
        .WithName("Ping").WithTags("Minimal").AllowAnonymous();

    Log.Information("Location Services listening on http://localhost:7136");
    app.Run();
}
catch (Exception ex)  { Log.Fatal(ex, "Application failed to start."); }
finally               { Log.CloseAndFlush(); }
