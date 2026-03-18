using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient("EligibilityApi", client =>
        {
            client.BaseAddress = new Uri(
                context.Configuration["EligibilityApiBaseUrl"] ?? "https://localhost:5001");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    })
    .Build();

host.Run();
