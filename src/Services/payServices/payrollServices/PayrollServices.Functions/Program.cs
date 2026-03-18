using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PayrollServices.Infrastructure.Data;

namespace PayrollServices.Functions;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services =>
            {
                // Configure Application Insights (optional)
                // services.AddApplicationInsightsTelemetry();
                services.AddDbContext<PayrollDbContext>();
            })
            .Build();

        host.Run();
    }
}
