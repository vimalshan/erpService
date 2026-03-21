namespace BookingService.Domain.Enums;

public enum BookingType
{
    Stay = 'S',
    Travel = 'T',
    LocalConveyance = 'L'
}

public enum BookingStatus
{
    New = 'N',
    Confirmed = 'C',
    CancellationRequested = 'K',
    Applied = 'A'
}

public enum TravelArrangement
{
    Self = 'Y',
    Admin = 'N',
    Coupon = 'C'
}

public enum PersonStatus
{
    Self = 'S',
    Guest = 'G'
}

public enum ConfirmationStatus
{
    New = 'N',
    Confirmed = 'Y',
    CancellationRequested = 'K',
    Cancelled = 'C'
}

public enum TravelType
{
    OwnArrangement = 1,
    SponsoredByOthers = 2
}

public enum CouponUsageFlag
{
    Available = 'A',
    Used = 'U',
    Expired = 'E'
}
