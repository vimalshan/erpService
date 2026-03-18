using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TdsService.Application.Vendors.Queries.GetAllTdsVendors;

namespace TdsService.Functions;

/// <summary>
/// Timer-triggered function that runs daily to sync/audit TDS vendor data.
/// Useful for reconciling vendor records, detecting stale PAN entries, etc.
/// </summary>
public sealed class TdsVendorAuditFunction
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TdsVendorAuditFunction> _logger;

    public TdsVendorAuditFunction(
        IServiceScopeFactory scopeFactory,
        ILogger<TdsVendorAuditFunction> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    [Function(nameof(TdsVendorAuditFunction))]
    public async Task Run(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timer,  // daily at 02:00 UTC
        CancellationToken ct = default)
    {
        _logger.LogInformation("TdsVendorAuditFunction triggered at: {Time}", DateTimeOffset.UtcNow);

        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var vendors = await mediator.Send(new GetAllTdsVendorsQuery(1, 1000), ct);

        var missingPan = vendors.Items.Where(v => string.IsNullOrEmpty(v.PanNo)).ToList();
        var missingEmail = vendors.Items.Where(v => string.IsNullOrEmpty(v.EmailAddress)).ToList();

        _logger.LogInformation(
            "Audit complete. Total={Total}, MissingPAN={MissingPAN}, MissingEmail={MissingEmail}",
            vendors.TotalCount, missingPan.Count, missingEmail.Count);

        if (missingPan.Count > 0)
        {
            _logger.LogWarning("Vendors missing PAN: {Vendors}",
                string.Join(", ", missingPan.Select(v => $"{v.VendorId}:{v.VendorName}")));
        }
    }
}
