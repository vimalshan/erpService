using MasterDataService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Functions;

public class ExpiredCouponCleanupFunction
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpiredCouponCleanupFunction> _logger;

    public ExpiredCouponCleanupFunction(IUnitOfWork unitOfWork, ILogger<ExpiredCouponCleanupFunction> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [Function("ExpiredCouponCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Expired coupon cleanup started at: {Time}", DateTime.UtcNow);

        var expiredCoupons = await _unitOfWork.Coupons.GetExpiredCouponsAsync();
        _logger.LogInformation("Found {Count} expired coupons", expiredCoupons.Count);

        foreach (var coupon in expiredCoupons)
        {
            await _unitOfWork.Coupons.DeleteAsync(coupon);
        }

        if (expiredCoupons.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Cleaned up {Count} expired coupons", expiredCoupons.Count);
        }
    }
}
