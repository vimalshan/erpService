using HealthChecks.SqlServer;
using LovService.API.Extensions;
using LovService.API.GraphQL;
using LovService.API.Middleware;
using LovService.API.MinimalApis;
using LovService.Application;
using LovService.Infrastructure;
using LovService.Infrastructure.Data;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Application & Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Auth
builder.Services.AddJwtAuthentication(builder.Configuration);

// REST Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI (Scalar)
builder.Services.AddOpenApiWithScalar();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<LovQuery>()
    .AddMutationType<LovMutation>()
    .AddType<LovTypeMastType>()
    .AddType<LovMasterType>()
    .AddType<ProgramLovMastType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddAuthorization()
    .BindRuntimeType<char, StringType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

// Health Checks
builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("LovDb")!,
        name: "sqlserver",
        tags: ["db", "sql"]);

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LovDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

// OpenAPI / Scalar UI (always available – restrict in prod via auth/network policy)
app.MapOpenApi();
app.MapScalarApiReference("/swagger/index.html");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

app.MapLovEndpoints();

app.Run();
