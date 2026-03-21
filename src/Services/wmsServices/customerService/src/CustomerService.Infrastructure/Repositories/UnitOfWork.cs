using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.Persistence;

namespace CustomerService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CustomerDbContext _context;
    private ICustomerRepository? _customers;

    public UnitOfWork(CustomerDbContext context)
    {
        _context = context;
    }

    public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
