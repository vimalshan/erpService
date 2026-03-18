using Microsoft.EntityFrameworkCore;
using TrainingDevelopment.Domain.Entities;
using TrainingDevelopment.Domain.Interfaces;

namespace TrainingDevelopment.Infrastructure.Repositories;

public class TrainingDetailRepository : ITrainingDetailRepository
{
    private readonly Data.ApplicationDbContext _context;

    public TrainingDetailRepository(Data.ApplicationDbContext context) => _context = context;

    public async Task<TrainingDetail?> GetByIdAsync(decimal id, CancellationToken cancellationToken = default)
        => await _context.TrainingDetails.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<TrainingDetail>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TrainingDetails.ToListAsync(cancellationToken);

    public async Task<IEnumerable<TrainingDetail>> GetByEmployeeAsync(decimal employeeSysId, CancellationToken cancellationToken = default)
        => await _context.TrainingDetails.Where(x => x.EmployeeSysId == employeeSysId).ToListAsync(cancellationToken);

    public async Task<IEnumerable<TrainingDetail>> GetByFinancialYearAsync(decimal year, CancellationToken cancellationToken = default)
        => await _context.TrainingDetails.Where(x => x.FinancialYear == year).ToListAsync(cancellationToken);

    public async Task<IEnumerable<TrainingDetail>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
        => await _context.TrainingDetails.Where(x => x.Status == status).ToListAsync(cancellationToken);

    public async Task AddAsync(TrainingDetail entity, CancellationToken cancellationToken = default)
        => await _context.TrainingDetails.AddAsync(entity, cancellationToken);

    public void Update(TrainingDetail entity)
        => _context.TrainingDetails.Update(entity);

    public void Delete(TrainingDetail entity)
        => _context.TrainingDetails.Remove(entity);
}
