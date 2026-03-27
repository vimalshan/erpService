namespace ApiGateway.API.Configuration;

public sealed class ServiceEndpoints
{
    public const string SectionName = "ServiceEndpoints";

    public string CashManagement { get; set; } = string.Empty;
    public string CurrencyManagement { get; set; } = string.Empty;
    public string DealTicketing { get; set; } = string.Empty;
    public string LoanManagement { get; set; } = string.Empty;
    public string OrganizationSetup { get; set; } = string.Empty;
    public string EmailNotification { get; set; } = string.Empty;

    public Dictionary<string, string> GetAll() => new()
    {
        [nameof(CashManagement)] = CashManagement,
        [nameof(CurrencyManagement)] = CurrencyManagement,
        [nameof(DealTicketing)] = DealTicketing,
        [nameof(LoanManagement)] = LoanManagement,
        [nameof(OrganizationSetup)] = OrganizationSetup,
        [nameof(EmailNotification)] = EmailNotification
    };
}
