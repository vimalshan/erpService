using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GroupManagementService.Domain.Repositories;
using GroupManagementService.Infrastructure.Persistence;
using GroupManagementService.Infrastructure.Repositories;

namespace GroupManagementService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Register DbContext
            services.AddDbContext<GroupManagementDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
                    sqlOptions.CommandTimeout(300);
                })
            );

            // Register repositories
            services.AddScoped<IGroupRepository, GroupRepository>();

            return services;
        }
    }
}
