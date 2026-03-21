using System.Text;
using ExpenseService.API.Auth;
using ExpenseService.API.Endpoints;
using ExpenseService.API.GraphQL;
using ExpenseService.API.GraphQL.Types;
using ExpenseService.API.Middleware;
using ExpenseService.API.Policies;
using ExpenseService.Application;
using ExpenseService.Infrastructure;
using ExpenseService.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ─── Application & Infrastructure DI ───────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ─── Controllers ───────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── JWT Authentication ────────────────────────────────────────────
builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();

// ─── Swagger / OpenAPI ─────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── GraphQL (Hot Chocolate) ───────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ExpenseQuery>()
    .AddMutationType<ExpenseMutation>()
    .AddType<ExpenseType>()
    .AddType<DaSummaryType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// ─── Polly Circuit Breaker for HttpClient ──────────────────────────
builder.Services.AddHttpClient("ExternalService")
    .AddPolicyHandler(PollyPolicies.GetCombinedPolicy());

// ─── Health Checks ─────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: ["db", "sql", "ready"]);

// ─── MediatR for API event handlers ───────────────────────────────
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// ─── Middleware Pipeline ───────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Service API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ─── Map Endpoints ─────────────────────────────────────────────────
app.MapControllers();
app.MapGraphQL("/graphql");
app.MapExpenseEndpoints();
app.MapHealthChecks("/health");

// ─── Seed Data ─────────────────────────────────────────────────────
await SeedData.InitializeAsync(app.Services);

app.Run();

