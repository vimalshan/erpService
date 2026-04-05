using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using TransactionService.API.Endpoints;
using TransactionService.API.GraphQL;
using TransactionService.API.HealthChecks;
using TransactionService.API.Middleware;
using TransactionService.Application;
using TransactionService.Infrastructure;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Seeds;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "Transaction Service API";
        document.Info.Description = "Manages approval workflows, transaction logging, and cross-service orchestration.";
        return Task.CompletedTask;
    });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

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
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<TransactionQuery>()
    .AddMutationType<TransactionMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

builder.Services.AddTransactionHealthChecks(builder.Configuration);

builder.Services.AddHttpClient("TransactionHttpClient");

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Transaction Service API")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql");
app.MapTransactionHealthChecks();
app.MapApprovalWorkflowEndpoints();
app.MapTransactionLogEndpoints();

app.MapGet("/api/v1/ping", () => Results.Ok(new { message = "Transaction Service is running.", timestamp = DateTime.UtcNow }))
   .WithName("Ping")
   .WithTags("Health")
   .AllowAnonymous();

app.MapGet("/api/rabbitmq/test", (IServiceProvider sp, IConfiguration config) =>
{
    try
    {
        var publisher = sp.GetRequiredService<TransactionService.Domain.Interfaces.IMessagePublisher>();
        return Results.Ok(new { service = "RabbitMQ", status = "Available", host = config["RabbitMQ:Host"] ?? "localhost" });
    }
    catch
    {
        return Results.Ok(new { service = "RabbitMQ", status = "Disconnected", host = config["RabbitMQ:Host"] ?? "localhost" });
    }
}).AllowAnonymous();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    db.Database.Migrate();
    await TransactionDbSeed.SeedAsync(db);
}

app.Run();
