using MediatR;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using MasterData.Application.Behaviors;
using MasterData.Application.Mappings;
using MasterData.Infrastructure;
using MasterData.API.Middleware;
using HotChocolate.Execution.Configuration;

namespace MasterData.API
{
    /// <summary>
    /// Startup configuration for the API
    /// </summary>
    public static class StartupConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
            var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
            var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/masterdata-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Register services
            builder.Services.AddInfrastructureServices(configuration.GetConnectionString("DefaultConnection")!);

            // Register MediatR
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.RegisterServicesFromAssembly(typeof(MasterData.Application.Handlers.CompanyUnit.GetAllCompanyUnitsHandler).Assembly);
            });

            // Register pipeline behaviors
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Register FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

            // Configure JWT Authentication
            var key = Encoding.ASCII.GetBytes(secret);
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
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            // Register controllers
            builder.Services.AddControllers();

            // Register GraphQL
            builder.Services
                .AddGraphQLServer()
                .AddQueryType<MasterData.API.GraphQL.Query>()
                .AddMutationType<MasterData.API.GraphQL.Mutation>()
                .RegisterService<MediatR.IMediator>();

            // Register Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "Master Data Service API", 
                    Version = "v1",
                    Description = "Comprehensive REST API for Master Data Management"
                });

                var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });
            });

            // Register Health Checks
            builder.Services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!);

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return builder;
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            // Use exception handling middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();

            // Use Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Master Data Service API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            // Map health check endpoint
            app.MapHealthChecks("/health");

            // Map controllers
            app.MapControllers();

            // Map GraphQL endpoint
            app.MapGraphQL("/graphql");

            return app;
        }
    }
}
