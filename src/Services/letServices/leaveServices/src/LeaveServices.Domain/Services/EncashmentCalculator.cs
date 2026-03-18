namespace LeaveServices.Domain.Services;

/// <summary>
/// Domain Service: calculates leave encashment amount.
/// Mirrors the logic in fn_Leave_CalculateEncashment.
/// </summary>
public static class EncashmentCalculator
{
    private const decimal EncashmentRate = 0.5m;
    private const int MonthsPerYear = 12;
    private const int DaysPerYear = 365;

    /// <summary>
    /// Calculates encashment amount = (BasicSalary * 12 / 365) * days * 0.5
    /// </summary>
    public static decimal Calculate(decimal basicSalary, int days)
    {
        if (basicSalary < 0) throw new ArgumentOutOfRangeException(nameof(basicSalary));
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days));

        var dailyWage = basicSalary * MonthsPerYear / DaysPerYear;
        return Math.Round(dailyWage * days * EncashmentRate, 2);
    }
}
