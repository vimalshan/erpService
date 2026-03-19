using UserService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// RabbitMQ
builder.Services.AddRabbitMqServices(builder.Configuration);

var app = builder.Build();

// Use pipeline
app.UseCors("AllowAll");
await app.UseApplicationPipeline();

app.Run();
