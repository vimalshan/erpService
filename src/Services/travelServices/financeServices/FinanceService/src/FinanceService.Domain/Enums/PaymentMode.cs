namespace FinanceService.Domain.Enums;

public enum PaymentMode
{
    Cheque,
    Bank,
    Cash
}

public static class PaymentModeExtensions
{
    public static string ToCode(this PaymentMode mode) => mode switch
    {
        PaymentMode.Cheque => "CHQ",
        PaymentMode.Bank => "BNK",
        PaymentMode.Cash => "CSH",
        _ => throw new ArgumentOutOfRangeException(nameof(mode))
    };

    public static PaymentMode FromCode(string code) => code switch
    {
        "CHQ" => PaymentMode.Cheque,
        "BNK" => PaymentMode.Bank,
        "CSH" => PaymentMode.Cash,
        _ => throw new ArgumentOutOfRangeException(nameof(code))
    };
}
