using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using SalesOrderService.Application;
using SalesOrderService.Infrastructure;
using SalesOrderService.Infrastructure.Persistence.Seeds;
using SalesOrderService.API.Middleware;
using SalesOrderService.API.Endpoints;
using SalesOrderService.API.GraphQL;

// ── Serilog ──────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration));

    var config = builder.Configuration;

    // ── Application & Infrastructure services ────────────────────────────────
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(config);

    // ── Controllers ──────────────────────────────────────────────────────────
    builder.Services.AddControllers();

    // ── Swagger / OpenAPI ────────────────────────────────────────────────────
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo
        {
            Title   = "Sales Order Service API",
            Version = "v1",
            Description = "WMS Sales Order microservice — REST, Minimal APIs, and GraphQL"
        });

        // JWT auth in Swagger UI
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name         = "Authorization",
            Type         = SecuritySchemeType.Http,
            Scheme       = "Bearer",
            BearerFormat = "JWT",
            In           = ParameterLocation.Header,
            Description  = "Enter: Bearer {your JWT token}"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // ── JWT Authentication ────────────────────────────────────────────────────
    var jwt = config.GetSection("JwtSettings");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = jwt["Issuer"],
                ValidAudience            = jwt["Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt["SecretKey"]!))
            };
        });

    builder.Services.AddAuthorization();

    // ── GraphQL (HotChocolate) ────────────────────────────────────────────────
    builder.Services
        .AddGraphQLServer()
        .BindRuntimeType<DateTime, FlexibleDateTimeType>()
        .AddQueryType<SalesOrderService.API.GraphQL.SalesOrderQuery>()
        .AddMutationType<SalesOrderService.API.GraphQL.SalesOrderMutation>();

    // ── Health Checks ─────────────────────────────────────────────────────────
    builder.Services
        .AddHealthChecks()
        .AddSqlServer(
            config.GetConnectionString("SalesOrderDb")!,
            name: "sqlserver",
            tags: ["db", "sql"]);

    // ── Build ─────────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ── Initialise database ───────────────────────────────────────────────────
    await DatabaseInitializer.InitializeAsync(app.Services);

    // ── Middleware pipeline ───────────────────────────────────────────────────
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales Order Service v1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapSalesOrderEndpoints();        // Minimal API routes
    app.MapGraphQL("/graphql");           // Banana Cake Pop / GraphQL endpoint

    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}
