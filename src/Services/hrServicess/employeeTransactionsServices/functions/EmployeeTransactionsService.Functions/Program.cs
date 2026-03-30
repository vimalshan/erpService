using EmployeeTransactionsService.Application;
using EmployeeTransactionsService.Infrastructure;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults()
	.ConfigureServices((context, services) =>
	{
		services.AddApplication();
		services.AddInfrastructure(context.Configuration);
	})
	.Build();

await host.RunAsync();
