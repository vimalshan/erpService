using BusServices.Infrastructure.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusServices.Functions.Functions;

/// <summary>
/// Runs monthly on the 1st at 2am to calculate deductions for all employees.
/// </summary>
public sealed class DeductionCalculatorFunction
{
    private readonly BusDbContext _ctx;
    private readonly ILogger<DeductionCalculatorFunction> _logger;

    public DeductionCalculatorFunction(BusDbContext ctx, ILogger<DeductionCalculatorFunction> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    [Function("DeductionCalculator")]
    public async Task Run([TimerTrigger("0 0 2 1 * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("DeductionCalculator triggered at: {Time}", DateTime.UtcNow);

        var today = DateTime.UtcNow.Date;
        var assignments = await _ctx.EmployeeBusAssignments
            .Where(e => e.ClosingDate == null || e.ClosingDate >= today)
            .ToListAsync(ct);

        _logger.LogInformation("Processing deductions for {Count} active assignments.", assignments.Count);

        foreach (var assignment in assignments)
        {
            var activeRate = await _ctx.BusDeductionRates
                .Where(d => d.BusId == assignment.BusId
                    && d.EffectiveDate <= today
                    && (d.ClosingDate == null || d.ClosingDate >= today))
                .OrderByDescending(d => d.EffectiveDate)
                .FirstOrDefaultAsync(ct);

            if (activeRate is not null)
            {
                _logger.LogInformation(
                    "Employee={EmpSysId}, Bus={BusId}, DeductionAmount={Amount}",
                    assignment.EmpSysId, assignment.BusId, activeRate.Amount);
                // TODO: Integrate with Payroll service to apply deduction
            }
        }

        _logger.LogInformation("DeductionCalculator completed.");
    }
}
