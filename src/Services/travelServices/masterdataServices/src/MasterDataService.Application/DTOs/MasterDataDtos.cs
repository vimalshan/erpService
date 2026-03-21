namespace MasterDataService.Application.DTOs;

public record GuestHouseDto(long Id, long AdminCode, string GuestHouseName, string Type, long DailyAmount);
public record GuestHouseRoomDto(long Id, long GuestHouseCode, long RoomSerial, long NumberOfPersons, long RoomNumber, long Floor);
public record GuestRoomAvailabilityDto(long Id, long FloorNumber, long RoomNumber, char RoomStatus, string? FloorValue);
public record GlCodeCombinationDto(long Id, long RowId, long CodeCombinationId, long ChartOfAccountsId, string ConcatenatedSegments, string AccountType, bool EnabledFlag, bool SummaryFlag, string? Context);
public record CouponDto(long Id, long CouponId, string? Airline, long TotalCoupons, long UsedCoupons, long BalanceCoupons, DateTime? ValidTill);
public record TaxSlabDto(long Id, string TaxType, DateTime EffectiveDate, DateTime? CloseDate, decimal TaxRate, long VendorCode);
public record AreaDto(long Id, int AreaId, string AreaName);
public record RouteDto(long Id, int RouteId, string RouteName);
