using AttendanceService.Application;
using AttendanceService.Functions;
using AttendanceService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure(ctx.Configuration);
        services.AddHostedService<AttendanceBatchFunction>();
        services.AddHostedService<SwipePunchProcessorFunction>();
    })
    .Build();

await host.RunAsync();
