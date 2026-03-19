using BatchAndEnvelopeService.API.Auth;
using BatchAndEnvelopeService.API.GraphQL;
using BatchAndEnvelopeService.API.Middleware;
using BatchAndEnvelopeService.Application;
using BatchAndEnvelopeService.Infrastructure;
using BatchAndEnvelopeService.Infrastructure.Persistence;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ────────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ── Application layers ─────────────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── REST API ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ── Swagger ────────────────────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Batch & Envelope Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// ── JWT Authentication ─────────────────────────────────────────────────────
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── GraphQL (HotChocolate) ─────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<BatchQuery>()
    .AddTypeExtension<EnvelopeQuery>()
    .AddMutationType<BatchMutation>()
    .AddTypeExtension<EnvelopeMutation>();

// ── Health Checks ──────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "database",
        tags: new[] { "db", "sql" })
    .AddRabbitMQ(
        _ =>
        {
            var factory = new ConnectionFactory
            {
                HostName = builder.Configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = builder.Configuration["RabbitMQ:Username"] ?? "guest",
                Password = builder.Configuration["RabbitMQ:Password"] ?? "guest"
            };
            return factory.CreateConnectionAsync();
        },
        name: "rabbitmq",
        tags: new[] { "messaging" });

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// ── Migrate & Seed DB ──────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try { await DbSeeder.SeedAsync(db); }
    catch (Exception ex) { Log.Warning(ex, "DB seed/migration failed – continuing startup"); }
}

// ── Middleware Pipeline ────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Batch & Envelope Service v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapGet("/api/v1/status", () => new { Status = "OK", Timestamp = DateTime.UtcNow })
    .WithName("GetStatus").WithTags("Health").AllowAnonymous();

app.MapPost("/api/v1/auth/token", (TokenRequest request, IConfiguration config) =>
{
    // Demo endpoint — in production, validate credentials against a user store
    if (request.Username == "admin" && request.Password == "admin123")
    {
        var token = JwtExtensions.GenerateToken(request.Username, "Admin", config);
        return Results.Ok(new { Token = token });
    }
    return Results.Unauthorized();
})
.WithName("GetToken").WithTags("Auth").AllowAnonymous();

app.Run();

record TokenRequest(string Username, string Password);

