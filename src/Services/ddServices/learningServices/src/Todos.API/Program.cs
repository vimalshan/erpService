using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Serilog;
using Todos.Application.Behaviors;
using Todos.Application.Mappers;
using Todos.Domain;
using Todos.Infrastructure.MessageBrokers;
using Todos.Infrastructure.Persistence;
using Todos.Infrastructure.Repositories;
using Azure.Storage.Blobs;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Todos.API.GraphQL.Learning;
using FluentValidation;

namespace Todos.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Serilog configuration
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/todos-.txt", rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();

        // Add services to the container
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<TodosDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__TodosMigrationsHistory");
                sqlOptions.MigrationsAssembly("Todos.Infrastructure");
            }));

        // Register UnitOfWork and Repositories
        builder.Services.AddScoped<IUnitOfWork, Infrastructure.Persistence.UnitOfWork>();
        builder.Services.AddScoped<IRepository<Domain.Entities.LearningRecord>, LearningRecordRepository>();
        builder.Services.AddScoped<IRepository<Domain.Entities.LearningFeedback>, LearningFeedbackRepository>();
        builder.Services.AddScoped<IRepository<Domain.Entities.DevelopmentCategoryDetail>, Infrastructure.Persistence.EFRepository<Domain.Entities.DevelopmentCategoryDetail>>();
        builder.Services.AddScoped<IRepository<Domain.Entities.LearningSubRecord>, Infrastructure.Persistence.EFRepository<Domain.Entities.LearningSubRecord>>();

        // Register specific repositories
        builder.Services.AddScoped<LearningRecordRepository>();
        builder.Services.AddScoped<LearningFeedbackRepository>();

        // Add AutoMapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Add Fluent Validation
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddValidatorsFromAssembly(typeof(Todos.Application.Queries.GetAllLearningRecordsQuery).Assembly);

        // Add MediatR
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();
            config.RegisterServicesFromAssembly(typeof(Todos.Application.Queries.GetAllLearningRecordsQuery).Assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(PerformanceMonitoringBehavior<,>));
        });

        // Configure RabbitMQ
        var rabbitmqConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();
        builder.Services.AddSingleton(rabbitmqConfig ?? new RabbitMQConfiguration());
        builder.Services.AddSingleton<IConnectionFactory>(sp =>
        {
            var config = sp.GetRequiredService<RabbitMQConfiguration>();
            return new ConnectionFactory
            {
                HostName = config.HostName ?? "localhost",
                Port = config.Port,
                UserName = config.UserName ?? "guest",
                Password = config.Password ?? "guest",
                VirtualHost = config.VirtualHost ?? "/"
            };
        });
        builder.Services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

        // Configure Azure Blob Storage
        var blobConfig = builder.Configuration.GetSection("BlobStorage").Get<BlobStorageConfiguration>();
        builder.Services.AddSingleton(blobConfig ?? new BlobStorageConfiguration());
        builder.Services.AddSingleton(x =>
        {
            var config = x.GetRequiredService<BlobStorageConfiguration>();
            return new BlobServiceClient(config.ConnectionString);
        });
        builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

        // Add JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "your-super-secret-key-that-is-very-long");

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
                ValidateLifetime = true
            };
        });

        // Add Authorization
        builder.Services.AddAuthorization();

        // Add Health Checks
        builder.Services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "SQL Server");
            // RabbitMQ is optional - omitted from health checks since NoOp publisher handles unavailability gracefully

        // Add Controllers
        builder.Services.AddControllers();

        // Add Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Learning & Training Service API",
                Version = "v1",
                Description = "API for managing learning and training records",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Development Team",
                    Email = "dev@example.com"
                }
            });

            // Add JWT to Swagger
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });

        // Add GraphQL (HotChocolate)
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<LearningQuery>()
            .AddMutationType<LearningMutation>()
            .AddSubscriptionType<LearningSubscription>();

        var app = builder.Build();

        // Migrate database
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TodosDbContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline
        // Enable Swagger for both Development and Production
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentname}/swagger.json";
        });
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning API v1");
            options.RoutePrefix = "swagger";  // Serve at /swagger
        });

        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        // Add Health Checks endpoint
        app.MapHealthChecks("/health");

        // Add GraphQL endpoint
        app.MapGraphQL("/graphql");

        app.MapControllers();

        app.Run();
    }
}
