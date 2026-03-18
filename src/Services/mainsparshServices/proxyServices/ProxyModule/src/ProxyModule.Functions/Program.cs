using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ProxyModule.Infrastructure.Persistence;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var connStr = context.Configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<ProxyModuleDbContext>(options =>
            options.UseSqlServer(connStr));
    })
    .Build();

host.Run();
