namespace InvoiceProcessing.Domain.ValueObjects;

public record AccountCode(string BusinessUnit, string LocationCode, string Account, string SubAccount, string CostCenter, string Product)
{
    public override string ToString() => $"{BusinessUnit}-{LocationCode}-{Account}-{SubAccount}-{CostCenter}-{Product}";
}
