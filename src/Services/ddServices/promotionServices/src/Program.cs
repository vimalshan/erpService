using System.Text;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using HealthChecks.UI.Client;
using Asp.Versioning;
using PromotionService.Features.Behaviours;
using PromotionService.Infrastructure.Auth;
using PromotionService.Infrastructure.BlobStorage;
using PromotionService.Infrastructure.HealthChecks;
using PromotionService.Infrastructure.Messaging;
using PromotionService.Infrastructure.Persistence;
using PromotionService.Infrastructure.Repositories;
using PromotionService.Infrastructure.UnitOfWork;
using PromotionService.Mapping;
using PromotionService.Middleware;
using PromotionService.MinimalApis;
using PromotionService.Schema.Mutations;
using PromotionService.Schema.Queries;
using PromotionService.Types;

// ── Serilog bootstrap ──────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ──────────────────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, services, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
           .ReadFrom.Services(services)
           .Enrich.FromLogContext()
           .Enrich.WithProperty("ServiceName", "PromotionService")
           .WriteTo.Console()
           .WriteTo.File("logs/promotion-service-.txt", rollingInterval: RollingInterval.Day));

    var config = builder.Configuration;

    // ── EF Core DbContext ────────────────────────────────────────────────
    builder.Services.AddDbContext<PromotionDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
            sql => sql.MigrationsAssembly("PromotionService")));

    // ── Repository + Unit of Work ────────────────────────────────────────
    builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    builder.Services.AddScoped<IDapperRepository, DapperRepository>();

    // ── MediatR + Pipeline Behaviours ───────────────────────────────────
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
    });

    // ── FluentValidation ────────────────────────────────────────────────
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    // ── AutoMapper ───────────────────────────────────────────────────────
    builder.Services.AddAutoMapper(typeof(PromotionAutoMapperProfile));

    // ── Azure Blob Storage ───────────────────────────────────────────────
    builder.Services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

    // ── RabbitMQ Background Consumer ─────────────────────────────────────
    builder.Services.AddHostedService<PromotionRabbitMQConsumer>();

    // ── JWT Token Service ─────────────────────────────────────────────────
    builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

    // ── JWT Authentication ───────────────────────────────────────────────
    var jwtSettings = config.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"]
        ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();

    // ── API Versioning ───────────────────────────────────────────────────
    builder.Services.AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
    }).AddApiExplorer(opt =>
    {
        opt.GroupNameFormat = "'v'VVV";
        opt.SubstituteApiVersionInUrl = true;
    });

    // ── Controllers ──────────────────────────────────────────────────────
    builder.Services.AddControllers();

    // ── Swagger / OpenAPI ────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Promotion Service API",
            Version = "v1",
            Description = "DD Promotion and Increment management microservice"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer {token}'"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (System.IO.File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    });

    // ── GraphQL (HotChocolate) ───────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<PromotionQueries>()
        .AddMutationType<PromotionMutations>()
        .AddType<RatingType>()
        .AddType<PromotionRecommendationType>()
        .AddType<IncrementRequestType>()
        .AddType<VTCAssessmentType>();

    // ── Health Checks ────────────────────────────────────────────────────
    var rabbitHost = config["RabbitMq:HostName"] ?? "localhost";
    var rabbitPort = config["RabbitMq:Port"] ?? "5672";
    var rabbitUser = config["RabbitMq:UserName"] ?? "guest";
    var rabbitPass = config["RabbitMq:Password"] ?? "guest";
    var rabbitVHost = Uri.EscapeDataString(config["RabbitMq:VirtualHost"] ?? "/");

    builder.Services.AddHealthChecks()
        .AddDbContextCheck<PromotionDbContext>("EfCoreDb", tags: new[] { "db", "ready" })
        .AddCheck<PromotionServiceHealthCheck>("PromotionDomain", tags: new[] { "domain", "ready" })
        .AddRabbitMQ(
            rabbitConnectionString: $"amqp://{rabbitUser}:{rabbitPass}@{rabbitHost}:{rabbitPort}/{rabbitVHost}",
            name: "RabbitMQ",
            tags: new[] { "messaging", "ready" });

    // ── CORS ─────────────────────────────────────────────────────────────
    builder.Services.AddCors(options =>
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    // ── Application Insights ─────────────────────────────────────────────
    if (!string.IsNullOrWhiteSpace(config["ApplicationInsights:ConnectionString"]))
        builder.Services.AddApplicationInsightsTelemetry(config["ApplicationInsights:ConnectionString"]);

    // ── Build app ────────────────────────────────────────────────────────
    var app = builder.Build();

    // ── Seed database ────────────────────────────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        await SeedData.SeedAsync(scope.ServiceProvider.GetRequiredService<PromotionDbContext>());
    }

    // ── Middleware pipeline ──────────────────────────────────────────────
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Promotion Service v1"));
    }

    app.UseSerilogRequestLogging();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // GraphQL endpoint
    app.MapGraphQL("/graphql");

    // Health check endpoints
    app.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = _ => false,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = hc => hc.Tags.Contains("ready"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    // Minimal API endpoints
    app.MapPromotionMinimalApis();

    Log.Information("Promotion Service started successfully.");
    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
