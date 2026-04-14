using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PurchaseOrderService.API.GraphQL;
using PurchaseOrderService.API.Middleware;
using PurchaseOrderService.API.MinimalApis;
using PurchaseOrderService.Application;
using PurchaseOrderService.Infrastructure;
using PurchaseOrderService.Infrastructure.Persistence;
using PurchaseOrderService.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// JWT Authentication
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// GraphQL (Hot Chocolate)
builder.Services
    .AddGraphQLServer()
    .BindRuntimeType<DateTime, FlexibleDateTimeType>()
    .AddQueryType<PurchaseOrderQuery>()
    .AddMutationType<PurchaseOrderMutation>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "database",
        tags: new[] { "db", "sql" });

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PurchaseOrder Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapPurchaseOrderEndpoints();
app.MapGraphQL();

app.MapHealthChecks("/health");

// Auto-migrate and seed in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PurchaseOrderDbContext>();
    await db.Database.MigrateAsync();
    await PurchaseOrderDbSeeder.SeedAsync(db);
}

app.Run();
