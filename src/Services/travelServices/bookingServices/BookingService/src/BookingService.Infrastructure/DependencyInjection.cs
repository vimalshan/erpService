using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Data;
using BookingService.Infrastructure.DapperRepositories;
using BookingService.Infrastructure.Messaging;
using BookingService.Infrastructure.Repositories;
using BookingService.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("BookingDb"),
                sqlOpts => sqlOpts.EnableRetryOnFailure(3)));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BookingDbContext>());

        // EF Repositories
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IBookingConfirmationRepository, BookingConfirmationRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();

        // Dapper read-side
        services.AddScoped<IBookingReadRepository, BookingDapperRepository>();

        // RabbitMQ
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<BookingMessageConsumer>();

        // Azure Blob Storage
        services.Configure<BlobStorageOptions>(configuration.GetSection("BlobStorage"));
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
