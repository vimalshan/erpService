using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using LetTransactionService.API.BackgroundServices;
using LetTransactionService.API.GraphQL.Mutations;
using LetTransactionService.API.GraphQL.Queries;
using LetTransactionService.API.GraphQL.Types;
using LetTransactionService.API.HealthChecks;
using LetTransactionService.API.Middleware;
using LetTransactionService.API.MinimalApis;
using LetTransactionService.Application;
using LetTransactionService.Infrastructure;
using LetTransactionService.Infrastructure.Data;
using Serilog;

namespace LetTransactionService.API;

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
            var jwtIssuer   = jwtSection["Issuer"]   ?? "LetTransactionService";
            var jwtAudience = jwtSection["Audience"] ?? "LetTransactionServiceClients";

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

            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("ReviewerPolicy", p => p.RequireRole("Reviewer", "Admin"));
            });

            // ─── Controllers + OpenAPI (built-in .NET 10) ──────────────────
            builder.Services.AddControllers();
            builder.Services.AddOpenApi(opts =>
            {
                opts.AddDocumentTransformer((doc, ctx, ct) =>
                {
                    doc.Info.Title   = "LET Transaction Services API";
                    doc.Info.Version = "v1";
                    return Task.CompletedTask;
                });
            });

            // ─── GraphQL (HotChocolate) ─────────────────────────────────────
            builder.Services
                .AddGraphQLServer()
                .AddQueryType<LetTransactionQuery>()
                .AddMutationType<LetTransactionMutation>()
                .AddType<LetMainType>()
                .AddType<LetSubType>()
                .AddType<LetSummaryType>()
                .AddType<FeedbackMainType>()
                .AddType<FeedbackSubType>()
                .AddType<ReviewMainType>()
                .AddType<ReviewSubType>()
                .AddType<PendingReviewType>()
                .BindRuntimeType<char, HotChocolate.Types.StringType>()
                .BindRuntimeType<char?, HotChocolate.Types.StringType>()
                .AddTypeConverter<char, string>(c => c.ToString())
                .AddTypeConverter<string, char>(s => s.Length > 0 ? s[0] : ' ')
                .AddTypeConverter<char?, string>(c => c?.ToString() ?? string.Empty)
                .AddAuthorization();

            // ─── Health Checks ──────────────────────────────────────────────
            builder.Services
                .AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database")
                .AddCheck<RabbitMqHealthCheck>("rabbitmq", tags: ["messaging"])
                .AddSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")!,
                    name: "sql-server");

            // ─── CORS ───────────────────────────────────────────────────────
            builder.Services.AddCors(opts =>
                opts.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            // ─── Background Services ────────────────────────────────────────
            builder.Services.AddHostedService<LetCleanupService>();

            // ─────────────────────────────────────────────────────────────────
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
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapGraphQL("/graphql");
            app.MapLetTransactionEndpoints();

            // ─── Dev-only token endpoint for testing ────────────────────
            if (app.Environment.IsDevelopment())
            {
                app.MapPost("/auth/token", (IConfiguration config) =>
                {
                    var jwt = config.GetSection("Jwt");
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, "TestUser"),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Role, "Reviewer")
                    };
                    var token = new JwtSecurityToken(
                        issuer: jwt["Issuer"],
                        audience: jwt["Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds);
                    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }).AllowAnonymous().WithTags("Auth");
            }

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
