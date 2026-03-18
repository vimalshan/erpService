using EmployeeService.Shared.Messaging;
using EmployeeService.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var rabbitMqSection = builder.Configuration.GetSection("RabbitMQ");
var rabbitMqSettings = new RabbitMqSettings
{
	HostName = rabbitMqSection["HostName"] ?? "localhost",
	Port = int.TryParse(rabbitMqSection["Port"], out var port) ? port : 5672,
	UserName = rabbitMqSection["UserName"] ?? "guest",
	Password = rabbitMqSection["Password"] ?? "guest",
	VirtualHost = rabbitMqSection["VirtualHost"] ?? "/",
	EmployeeEventsQueueName = rabbitMqSection["EmployeeEventsQueueName"] ?? "employee.events",
	PublishRetryCount = int.TryParse(rabbitMqSection["PublishRetryCount"], out var publishRetryCount) ? publishRetryCount : 2,
	PublishRetryDelaySeconds = int.TryParse(rabbitMqSection["PublishRetryDelaySeconds"], out var publishRetryDelaySeconds) ? publishRetryDelaySeconds : 2,
	PublishCircuitBreakDurationSeconds = int.TryParse(rabbitMqSection["PublishCircuitBreakDurationSeconds"], out var publishCircuitBreakDurationSeconds) ? publishCircuitBreakDurationSeconds : 30,
	PublishCircuitMinimumThroughput = int.TryParse(rabbitMqSection["PublishCircuitMinimumThroughput"], out var publishCircuitMinimumThroughput) ? publishCircuitMinimumThroughput : 2,
	PublishCircuitFailureRatio = double.TryParse(rabbitMqSection["PublishCircuitFailureRatio"], out var publishCircuitFailureRatio) ? publishCircuitFailureRatio : 0.5,
	ConsumerRetryDelaySeconds = int.TryParse(rabbitMqSection["ConsumerRetryDelaySeconds"], out var consumerRetryDelaySeconds) ? consumerRetryDelaySeconds : 10
};
builder.Services.AddSingleton(rabbitMqSettings);
builder.Services.AddHostedService<RabbitMqEmployeeEventConsumer>();

var host = builder.Build();
await host.RunAsync();
