using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class Coupon : AuditableEntity
{
    public long CouponId { get; private set; }
    public string? Airline { get; private set; }
    public long TotalCoupons { get; private set; }
    public long UsedCoupons { get; private set; }
    public long BalanceCoupons { get; private set; }
    public DateTime? ValidTill { get; private set; }

    private Coupon() { }

    public Coupon(long couponId, string? airline, long totalCoupons, long usedCoupons, long balanceCoupons, DateTime? validTill)
    {
        CouponId = couponId;
        Airline = airline;
        TotalCoupons = totalCoupons;
        UsedCoupons = usedCoupons;
        BalanceCoupons = balanceCoupons;
        ValidTill = validTill;
    }

    public void UseCoupon()
    {
        if (BalanceCoupons <= 0) throw new InvalidOperationException("No coupons available.");
        UsedCoupons++;
        BalanceCoupons--;
    }

    public bool IsExpired() => ValidTill.HasValue && ValidTill.Value < DateTime.UtcNow;
}
