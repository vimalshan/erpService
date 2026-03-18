using Microsoft.EntityFrameworkCore;
using RequestServices.Domain.Aggregates;
using RequestServices.Domain.Entities;
using RequestServices.Domain.Interfaces;
using RequestServices.Infrastructure.Data;

namespace RequestServices.Infrastructure.Repositories;

public class RequestRepository(RequestDbContext context) : IRequestRepository
{
    public async Task<RequestMain?> GetByIdAsync(long requestId, CancellationToken ct = default)
        => await context.RequestMain
            .Include(r => r.SubRequests)
            .FirstOrDefaultAsync(r => r.RequestId == requestId, ct);

    public async Task<IEnumerable<RequestMain>> GetPendingBySuperviorAsync(
        string supervisorUser, CancellationToken ct = default)
        => await context.RequestMain
            .Include(r => r.SubRequests)
            .Where(r => r.SupervisorUser == supervisorUser
                     && r.SubRequests.Any(s => s.StatusCode == 'P' || s.StatusCode == 'S'))
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync(ct);

    public async Task AddAsync(RequestAggregate aggregate, CancellationToken ct = default)
    {
        var main = RequestMain.Create(
            aggregate.RequestId, aggregate.EmployeeUser,
            aggregate.RequestDate, aggregate.SupervisorUser);

        await context.RequestMain.AddAsync(main, ct);

        foreach (var sub in aggregate.SubRequests)
            await context.RequestSub.AddAsync(sub, ct);

        foreach (var app in aggregate.Approvals)
            await context.RequestApp.AddAsync(app, ct);
    }

    public async Task UpdateAsync(RequestAggregate aggregate, CancellationToken ct = default)
    {
        // Update sub-request status changes tracked by change tracker
        foreach (var sub in aggregate.SubRequests)
        {
            var existing = await context.RequestSub
                .FindAsync([sub.SerialNumber], ct);
            if (existing is not null)
                context.Entry(existing).CurrentValues.SetValues(sub);
        }

        foreach (var app in aggregate.Approvals)
        {
            var key = new object[] { app.RequestId, app.SerialNumber };
            var existingApp = await context.RequestApp.FindAsync(key, ct);
            if (existingApp is null)
                await context.RequestApp.AddAsync(app, ct);
        }

        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(long requestId, CancellationToken ct = default)
        => await context.RequestMain.AnyAsync(r => r.RequestId == requestId, ct);
}
