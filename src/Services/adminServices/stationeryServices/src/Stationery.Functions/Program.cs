using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Stationery.Infrastructure.Persistence;
using Stationery.Infrastructure.Repositories;
using Stationery.Infrastructure.Services;
using Stationery.Domain.Interfaces;
using Stationery.Application.Features.Requests.Commands;
using Stationery.Application.Common.Behaviors;
using Stationery.Application.Mappings;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // Database
        services.AddDbContext<StationeryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repository & UoW
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Blob Storage
        services.AddScoped<IBlobService, BlobService>();

        // MediatR with behaviours
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateRequestCommand).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<StationeryMappingProfile>());
    })
    .Build();

host.Run();
