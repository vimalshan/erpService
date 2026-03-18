namespace EligibilityService.Domain.Exceptions;

public class EligibilityNotFoundException : Exception
{
    public EligibilityNotFoundException(long canteenUnit, string shiftCode, decimal itemCode)
        : base($"EligibilityMaster not found for CanteenUnit={canteenUnit}, ShiftCode={shiftCode}, ItemCode={itemCode}.")
    { }
}

public class DuplicateEligibilityException : Exception
{
    public DuplicateEligibilityException(long canteenUnit, string shiftCode, decimal itemCode)
        : base($"EligibilityMaster already exists for CanteenUnit={canteenUnit}, ShiftCode={shiftCode}, ItemCode={itemCode}.")
    { }
}
