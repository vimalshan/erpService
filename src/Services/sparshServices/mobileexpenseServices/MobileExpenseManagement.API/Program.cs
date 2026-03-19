using MobileExpenseManagement.API.Extensions;
using MobileExpenseManagement.API.Middleware;
using MobileExpenseManagement.Application.Behaviors;
using MobileExpenseManagement.Application.Commands;
using MobileExpenseManagement.Application.Common.Interfaces;
using MobileExpenseManagement.Application.Common.Mapping;
using MobileExpenseManagement.Application.Queries;
using MobileExpenseManagement.Infrastructure.Data;
using MobileExpenseManagement.Infrastructure.Repositories;
using MobileExpenseManagement.Infrastructure.BlobStorage;
using MobileExpenseManagement.Infrastructure.Messaging;
using MobileExpenseManagement.API.GraphQL;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using RabbitMQ.Client;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Add services to the container
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Add repositories and unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateExpenseCommand).Assembly));

// Add CQRS behaviors
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateExpenseCommandValidator>();

// Add Blob Storage
builder.Services.AddSingleton(x => new BlobServiceClient(configuration.GetConnectionString("BlobStorageConnection")));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add RabbitMQ
var rabbitMqSection = configuration.GetSection("RabbitMQ");
var connectionFactory = new ConnectionFactory
{
    HostName = rabbitMqSection["HostName"] ?? "localhost",
    UserName = rabbitMqSection["Username"] ?? "guest",
    Password = rabbitMqSection["Password"] ?? "guest",
    Port = int.Parse(rabbitMqSection["Port"] ?? "5672")
};

builder.Services.AddSingleton<IRabbitMQConnection>(provider =>
    new RabbitMQConnection(connectionFactory, provider.GetRequiredService<ILogger<RabbitMQConnection>>()));
builder.Services.AddScoped<IMessageBus, RabbitMQMessageBus>();

// Add JWT Authentication
builder.Services.AddJwtAuthentication(configuration);

// Add Circuit Breaker Policy
builder.Services.AddCircuitBreakerPolicy();

// Add Health Checks
builder.Services.AddHealthChecks(configuration);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mobile Expense Management API",
        Version = "v1",
        Description = "API for managing mobile expenses",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Sparsh ERP",
            Email = "support@sparsh.com"
        }
    });

    // Add JWT to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mobile Expense Management API v1"));
}

app.UseHttpsRedirection();

// Add middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowAny");

app.UseRouting();

// Health check endpoint
app.MapHealthChecks("/health");

// GraphQL endpoint
app.MapGraphQL("/graphql");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
