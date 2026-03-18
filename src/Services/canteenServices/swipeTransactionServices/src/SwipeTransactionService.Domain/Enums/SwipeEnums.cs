namespace SwipeTransactionService.Domain.Enums;

public enum PunchType
{
    CheckIn = 'I',
    CheckOut = 'O'
}

public enum MealType
{
    Breakfast = 'B',
    Lunch = 'L',
    Dinner = 'D',
    Snack = 'S'
}

public enum UpdateStatus
{
    Pending = 'P',
    Processed = 'Y',
    Failed = 'F'
}
