using System.Text;
using AttendanceService.API.Auth;
using Microsoft.Extensions.Hosting;
using AttendanceService.API.GraphQL;
using AttendanceService.API.Middleware;
using AttendanceService.API.MinimalApis;
using AttendanceService.Application;
using AttendanceService.Infrastructure;
using AttendanceService.Infrastructure.Persistence;
using AttendanceService.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// ─── Serilog bootstrap ────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // ─── Application & Infrastructure DI ─────────────────────────────────────
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // Keep the host alive even if a BackgroundService (e.g. RabbitMQ consumer) faults
    builder.Services.Configure<HostOptions>(opts =>
        opts.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

    // ─── JWT Settings ─────────────────────────────────────────────────────────
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Section));
    builder.Services.AddSingleton<JwtTokenGenerator>();

    // ─── JWT Authentication ───────────────────────────────────────────────────
    var jwtSection = builder.Configuration.GetSection(JwtSettings.Section);
    var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

    builder.Services.AddAuthorization();

    // ─── Controllers & Swagger ────────────────────────────────────────────────
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Attendance Service API",
            Version = "v1",
            Description = "Biometric Punch & Attendance Management Microservice"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
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
    });

    // ─── GraphQL ──────────────────────────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<AttendanceQuery>()
        .AddMutationType<AttendanceMutation>()
        .AddAuthorization();

    // ─── Health Checks ────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks();

    // ─── CORS ─────────────────────────────────────────────────────────────────
    builder.Services.AddCors(opts =>
        opts.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    var app = builder.Build();

    // ─── Seed database ────────────────────────────────────────────────────────
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var seedLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        await DatabaseSeeder.SeedAsync(db, seedLogger);
    }

    // ─── Middleware pipeline ──────────────────────────────────────────────────
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    // ─── Swagger ──────────────────────────────────────────────────────────────
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Attendance Service v1"));
    }

    // ─── Endpoints ────────────────────────────────────────────────────────────
    app.MapControllers();
    app.MapGraphQL("/graphql");
    app.MapAttendanceEndpoints();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (HostAbortedException)
{
    // Intentional abort by EF Core design-time tooling — not a real failure, re-throw so
    // the tooling receives the correct exit code.
    throw;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
