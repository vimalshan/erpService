using LoanTransaction.Domain.Interfaces;
using LoanTransaction.Infrastructure.Data;
using LoanTransaction.Infrastructure.Messaging;
using LoanTransaction.Infrastructure.Repositories;
using LoanTransaction.Infrastructure.Services;
using LoanTransaction.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanTransaction.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LoanTransactionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ILoanInstallmentRepository, LoanInstallmentRepository>();
        services.AddScoped<ILoanSettlementRepository, LoanSettlementRepository>();
        services.AddScoped<ILoanLedgerRepository, LoanLedgerRepository>();

        services.AddScoped<IUnitOfWork, Infrastructure.UnitOfWork.UnitOfWork>();

        services.AddScoped<IEmiCalculatorService, EmiCalculatorService>();
        services.AddScoped<DapperLoanQueryService>();

        services.Configure<RabbitMQSettings>(
            configuration.GetSection("RabbitMQ"));
        services.AddSingleton<IMessageBus, RabbitMQMessageBus>();

        return services;
    }
}
