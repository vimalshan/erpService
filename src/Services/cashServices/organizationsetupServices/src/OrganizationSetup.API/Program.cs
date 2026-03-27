using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OrganizationSetup.API.Services;
using OrganizationSetup.Application;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "YourSuperSecretKeyFor256BitHmacSha256AlgorithmMustBeAtLeast32Characters";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "OrganizationSetupAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "OrganizationSetupClients";

// Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();

// Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

// Controllers & API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

// GraphQL — HotChocolate
builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<OrganizationSetup.API.GraphQL.OrganizationSetupQuery>()
    .AddTypeExtension<OrganizationSetup.API.GraphQL.OrganizationSetupMutation>()
    .AddAuthorization();

// Health Checks
builder.Services
    .AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CASHDB;Integrated Security=True;")
    .AddRabbitMQ(sp =>
    {
        var config = builder.Configuration.GetSection("RabbitMQ");
        var factory = new RabbitMQ.Client.ConnectionFactory
        {
            HostName = config["HostName"] ?? "localhost",
            Port = int.Parse(config["Port"] ?? "5672"),
            UserName = config["UserName"] ?? "guest",
            Password = config["Password"] ?? "guest"
        };
        return factory.CreateConnectionAsync();
    }, name: "rabbitmq");

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Health Check Endpoint
app.MapHealthChecks("/health");

app.MapControllers();
app.MapGraphQL("/graphql");
app.Run();
