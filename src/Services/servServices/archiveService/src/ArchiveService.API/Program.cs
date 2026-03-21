using System.Text;
using ArchiveService.API.GraphQL;
using ArchiveService.API.Middleware;
using ArchiveService.Application;
using ArchiveService.Infrastructure;
using ArchiveService.Infrastructure.Persistence;
using ArchiveService.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// GraphQL (Hot Chocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ArchiveQuery>()
    .AddMutationType<ArchiveMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ArchiveDbContext>("database");

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger / OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();
app.MapHealthChecks("/health");

// Minimal APIs
app.MapGet("/api/minimal/service-orders/{sernoDell}", async (string sernoDell, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new ArchiveService.Application.Features.ServiceOrders.Queries.GetServiceOrderByIdQuery(sernoDell));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Minimal API");

app.MapGet("/api/minimal/service-orders", async (int page, int pageSize, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new ArchiveService.Application.Features.ServiceOrders.Queries.GetServiceOrdersPagedQuery(page, pageSize));
    return Results.Ok(result);
}).WithTags("Minimal API");

app.MapGet("/api/minimal/toolkits/{id:long}", async (long id, MediatR.IMediator mediator) =>
{
    var result = await mediator.Send(new ArchiveService.Application.Features.ToolKits.Queries.GetToolKitByIdQuery(id));
    return result is not null ? Results.Ok(result) : Results.NotFound();
}).WithTags("Minimal API");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ArchiveDbContext>();
    await ArchiveDbSeeder.SeedAsync(context);
}

app.Run();
