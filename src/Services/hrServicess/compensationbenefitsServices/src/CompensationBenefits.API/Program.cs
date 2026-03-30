using System.Text;
using CompensationBenefits.API.Middleware;
using CompensationBenefits.API.MinimalApis;
using CompensationBenefits.Application;
using CompensationBenefits.Infrastructure;
using CompensationBenefits.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ── Application & Infrastructure layers ─────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]!;
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

// ── GraphQL (HotChocolate) ────────────────────────────────────────────────────
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CompensationBenefits.API.GraphQL.Query>()
    .AddMutationType<CompensationBenefits.API.GraphQL.Mutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ── Build ─────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompensationBenefits API v1"));
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ── REST Controllers ──────────────────────────────────────────────────────────
app.MapControllers();

// ── Minimal API endpoints ─────────────────────────────────────────────────────
app.MapSalaryEndpoints();
app.MapSalaryStructureEndpoints();

// ── GraphQL endpoint ──────────────────────────────────────────────────────────
app.MapGraphQL("/graphql");

// ── Health checks ─────────────────────────────────────────────────────────────
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// ── Auth helper: issue a JWT token (dev convenience) ─────────────────────────
app.MapPost("/api/auth/token", (TokenRequest req) =>
{
    // In production, validate credentials against a real user store.
    if (req.Username != "admin" || req.Password != "admin123")
        return Results.Unauthorized();

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: jwtSection["Issuer"],
        audience: jwtSection["Audience"],
        expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiryMinutes"] ?? "60")),
        signingCredentials: creds
    );
    return Results.Ok(new { token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token) });
}).WithTags("Auth").AllowAnonymous();

// ── Database initialise ───────────────────────────────────────────────────────
await DbInitializer.InitialiseAsync(app.Services);

app.Run();

record TokenRequest(string Username, string Password);
