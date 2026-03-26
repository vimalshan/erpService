namespace AimsTransactionService.Domain.Enums;

/// <summary>Attendance batch processing status: N = New, Y = Completed, P = Processing.</summary>
public enum BatchStatus
{
    New = 'N',
    Processing = 'P',
    Completed = 'Y'
}
