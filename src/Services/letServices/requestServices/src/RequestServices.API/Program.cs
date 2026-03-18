
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using RequestServices.API.GraphQL.Mutations;
using RequestServices.API.GraphQL.Queries;
using RequestServices.API.GraphQL.Types;
using RequestServices.API.HealthChecks;
using RequestServices.API.Middleware;
using RequestServices.API.MinimalApis;
using RequestServices.Application;
using RequestServices.Infrastructure;
using RequestServices.Infrastructure.Data;
using Serilog;

namespace RequestServices.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
        var builder = WebApplication.CreateBuilder(args);

        // ─── Serilog ────────────────────────────────────────────────────
        builder.Host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        // ─── Application + Infrastructure layers ────────────────────────
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        // ─── JWT Authentication ─────────────────────────────────────────
        var jwtSection  = builder.Configuration.GetSection("Jwt");
        var jwtKey      = jwtSection["Key"]      ?? throw new InvalidOperationException("JWT Key not configured.");
        var jwtIssuer   = jwtSection["Issuer"]   ?? "RequestServices";
        var jwtAudience = jwtSection["Audience"] ?? "RequestServices";

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = jwtIssuer,
                    ValidAudience            = jwtAudience,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        builder.Services.AddAuthorization();

        // ─── Controllers + OpenAPI (built-in .NET 10) ──────────────────
        builder.Services.AddControllers();
        builder.Services.AddOpenApi(opts =>
        {
            opts.AddDocumentTransformer((doc, ctx, ct) =>
            {
                doc.Info.Title   = "Request Services API";
                doc.Info.Version = "v1";
                return Task.CompletedTask;
            });
        });

        // ─── GraphQL (Hot Chocolate) ────────────────────────────────────
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<RequestQuery>()
            .AddMutationType<RequestMutation>()
            .AddType<RequestMainType>()
            .AddType<RequestSubType>()
            .AddType<PendingRequestType>()
            .AddAuthorization();

        // ─── Health Checks ──────────────────────────────────────────────
        builder.Services
            .AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database")
            .AddSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")!,
                name: "sql-server");

        // ─── CORS ───────────────────────────────────────────────────────
        builder.Services.AddCors(opts =>
            opts.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        // ────────────────────────────────────────────────────────────────
        var app = builder.Build();

        // ─── DB Migration + Seed on startup ─────────────────────────────
        using (var scope = app.Services.CreateScope())
        {
            var initLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            await DatabaseInitializer.InitializeAsync(app.Services, initLogger);
        }

        // ─── Middleware pipeline ────────────────────────────────────────
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseCors();

        if (app.Environment.IsDevelopment())
        {
            // Built-in OpenAPI JSON endpoint at /openapi/v1.json
            app.MapOpenApi();
            // Scalar UI accessible at /scalar/v1
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapGraphQL("/graphql");
        app.MapRequestEndpoints();

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate      = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = hc => hc.Tags.Contains("ready") });
        app.MapHealthChecks("/health/live",  new HealthCheckOptions { Predicate = _ => false });

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
    }
}
