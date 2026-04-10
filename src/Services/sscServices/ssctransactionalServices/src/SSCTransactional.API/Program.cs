using SSCTransactional.API.Auth;
using SSCTransactional.API.GraphQL;
using SSCTransactional.API.Middleware;
using SSCTransactional.Application;
using SSCTransactional.Infrastructure;
using SSCTransactional.Infrastructure.DapperRepositories;
using SSCTransactional.Infrastructure.Persistence;
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
    c.SwaggerDoc("v1", new() { Title = "SSC Transactional Service API", Version = "v1" });
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
    .AddQueryType<AllocationQuery>()
    .AddTypeExtension<CorrespondenceQuery>()
    .AddTypeExtension<ApprovalQuery>()
    .AddTypeExtension<RescanQuery>()
    .AddTypeExtension<OracleQuery>()
    .AddMutationType<AllocationMutation>()
    .AddTypeExtension<CorrespondenceMutation>()
    .AddTypeExtension<ApprovalMutation>()
    .AddTypeExtension<RescanMutation>()
    .AddTypeExtension<RevokeMutation>();

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
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
    try { await DbSeeder.SeedAsync(db, logger); }
    catch (Exception ex) { Log.Warning(ex, "DB seed/migration failed – continuing startup"); }
}

// ── Middleware Pipeline ────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSC Transactional Service v1");
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

// ── Health Check endpoints ─────────────────────────────────────────────────
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

// ── Minimal API (status + auth) ────────────────────────────────────────────
app.MapGet("/api/v1/status", () => new { Status = "OK", Timestamp = DateTime.UtcNow })
    .WithName("GetStatus").WithTags("Health").AllowAnonymous();

app.MapPost("/api/v1/auth/token", (TokenRequest request, IConfiguration config) =>
{
    if (request.Username == "admin" && request.Password == "admin123")
    {
        var token = JwtExtensions.GenerateToken(request.Username, "Admin", config);
        return Results.Ok(new { Token = token });
    }
    return Results.Unauthorized();
})
.WithName("GetToken").WithTags("Auth").AllowAnonymous();

// ── Minimal API V2 (Dapper) ───────────────────────────────────────────────
var v2 = app.MapGroup("/api/v2").AllowAnonymous();

v2.MapGet("/allocations", async (AllocationDapperRepository repo, int page = 1, int pageSize = 20) =>
    Results.Ok(await repo.GetAllocationsPagedAsync(page, pageSize)))
    .WithName("GetAllocationsV2").WithTags("V2 Allocations");

v2.MapGet("/allocations/group/{groupId:long}", async (AllocationDapperRepository repo, long groupId, int page = 1, int pageSize = 20) =>
    Results.Ok(await repo.GetAllocationsByGroupAsync(groupId, page, pageSize)))
    .WithName("GetAllocationsByGroupV2").WithTags("V2 Allocations");

v2.MapGet("/allocations/doc/{docId:long}", async (AllocationDapperRepository repo, long docId) =>
    Results.Ok(await repo.GetAllocationsByDocIdAsync(docId)))
    .WithName("GetAllocationsByDocIdV2").WithTags("V2 Allocations");

v2.MapGet("/allocations/group/{groupId:long}/pending-count", async (AllocationDapperRepository repo, long groupId) =>
    Results.Ok(new { Count = await repo.GetPendingCountByGroupAsync(groupId) }))
    .WithName("GetPendingCountV2").WithTags("V2 Allocations");

v2.MapGet("/correspondences", async (CorrespondenceDapperRepository repo, int page = 1, int pageSize = 20) =>
    Results.Ok(await repo.GetCorrespondencesPagedAsync(page, pageSize)))
    .WithName("GetCorrespondencesV2").WithTags("V2 Correspondences");

v2.MapGet("/correspondences/doc/{docId:long}", async (CorrespondenceDapperRepository repo, long docId) =>
    Results.Ok(await repo.GetCorrespondencesByDocIdAsync(docId)))
    .WithName("GetCorrespondencesByDocIdV2").WithTags("V2 Correspondences");

v2.MapGet("/correspondences/{id:long}/attachments", async (CorrespondenceDapperRepository repo, long id) =>
    Results.Ok(await repo.GetAttachmentsByCorrespondenceIdAsync(id)))
    .WithName("GetCorrespondenceAttachmentsV2").WithTags("V2 Correspondences");

v2.MapGet("/correspondences/hold-count", async (CorrespondenceDapperRepository repo) =>
    Results.Ok(new { Count = await repo.GetActiveHoldCountAsync() }))
    .WithName("GetActiveHoldCountV2").WithTags("V2 Correspondences");

app.Run();

record TokenRequest(string Username, string Password);
