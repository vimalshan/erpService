namespace BookingService.Application.DTOs;

public class BookRequestMainDto
{
    public string BookMainId { get; set; } = null!;
    public string TpStatus { get; set; } = null!;
    public string TpId { get; set; } = null!;
    public string EmployeeSysId { get; set; } = null!;
    public string Through { get; set; } = null!;
    public string AdminId { get; set; } = null!;
    public string Remarks { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ApprovalStatus { get; set; } = null!;
    public string ConfirmationStatus { get; set; } = null!;
    public string? ProofType { get; set; }
    public string? FoodPreference { get; set; }
    public string? BudgetedCost { get; set; }
    public string? EnteredBy { get; set; }
    public DateTime? EnteredOn { get; set; }
    public string? EmployeeCalendarId { get; set; }
    public DateTime LastModifiedOn { get; set; }

    public List<BookRequestTicketDto> Tickets { get; set; } = [];
    public List<BookRequestStayDto> Stays { get; set; } = [];
    public List<BookRequestCabDto> Cabs { get; set; } = [];
    public List<BookRequestCostCentreDto> CostCentres { get; set; } = [];
    public List<BookRequestOtherDto> Others { get; set; } = [];
}

public class BookRequestTicketDto
{
    public string BookTicketId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string ModeId { get; set; } = null!;
    public string ClassId { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string StartTime { get; set; } = null!;
    public string StartCityId { get; set; } = null!;
    public string StartCity { get; set; } = null!;
    public string EndCityId { get; set; } = null!;
    public string EndCity { get; set; } = null!;
    public string ConfirmationNo { get; set; } = null!;
    public string ApprovalStatus { get; set; } = null!;
    public string BudgetCost { get; set; } = null!;
    public string SpecialSanction { get; set; } = null!;
    public string SpecialSanctionReason { get; set; } = null!;
}

public class BookRequestStayDto
{
    public string BookStayId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string CityId { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string ConfirmationNo { get; set; } = null!;
}

public class BookRequestCabDto
{
    public string BookCabId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string PickupLocation { get; set; } = null!;
    public string DropLocation { get; set; } = null!;
    public DateTime PickupDate { get; set; }
    public string? CarType { get; set; }
    public string Preference { get; set; } = null!;
    public string TripType { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ConfirmationNo { get; set; } = null!;
    public string? Nature { get; set; }
}

public class BookRequestCostCentreDto
{
    public string BookCcId { get; set; } = null!;
    public string MainId { get; set; } = null!;
    public string BusinessUnitCode { get; set; } = null!;
    public string CostCentreCode { get; set; } = null!;
    public string SubAccountCode { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string LocationSegment { get; set; } = null!;
    public string AllocationPercentage { get; set; } = null!;
}

public class BookRequestOtherDto
{
    public string BookOtherId { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string BookingFor { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Age { get; set; } = null!;
    public string ContactNo { get; set; } = null!;
    public string ApprovedBy { get; set; } = null!;
    public DateTime? ApprovedOn { get; set; }
}

// --- Input DTOs for creation (no server-generated fields) ---

public class CreateBookRequestTicketInput
{
    public string ModeId { get; set; } = null!;
    public string ClassId { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string StartTime { get; set; } = null!;
    public string StartCityId { get; set; } = null!;
    public string StartCity { get; set; } = null!;
    public string EndCityId { get; set; } = null!;
    public string EndCity { get; set; } = null!;
    public string? BudgetCost { get; set; }
    public string? SpecialSanction { get; set; }
    public string? SpecialSanctionReason { get; set; }
}

public class CreateBookRequestStayInput
{
    public string CityId { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}

public class CreateBookRequestCabInput
{
    public string PickupLocation { get; set; } = null!;
    public string DropLocation { get; set; } = null!;
    public DateTime PickupDate { get; set; }
    public string? CarType { get; set; }
    public string Preference { get; set; } = null!;
    public string TripType { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Nature { get; set; }
}

public class CreateBookRequestCostCentreInput
{
    public string BusinessUnitCode { get; set; } = null!;
    public string CostCentreCode { get; set; } = null!;
    public string SubAccountCode { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string LocationSegment { get; set; } = null!;
    public string AllocationPercentage { get; set; } = null!;
}

public class CreateBookRequestOtherInput
{
    public string BookingFor { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Age { get; set; } = null!;
    public string ContactNo { get; set; } = null!;
}

public class BookConfirmationDto
{
    public string BookConfId { get; set; } = null!;
    public string Mode { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string RefId { get; set; } = null!;
    public DateTime ConfirmationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Cost { get; set; } = null!;
    public string VendorId { get; set; } = null!;
    public string AdminRemarks { get; set; } = null!;
    public string ApprovalStatus { get; set; } = null!;
    public string? Attachment { get; set; }
}
