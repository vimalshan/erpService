using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: BENEFIT_MAST</summary>
public sealed class Benefit : AggregateRoot
{
    public string BenefitCode { get; private set; } = string.Empty;
    public string BenefitDescription { get; private set; } = string.Empty;

    private Benefit() { }

    public static Benefit Create(string benefitCode, string benefitDescription)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(benefitCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(benefitDescription);
        return new Benefit { BenefitCode = benefitCode.Trim().ToUpper(), BenefitDescription = benefitDescription.Trim() };
    }

    public void Update(string benefitDescription) => BenefitDescription = benefitDescription.Trim();
}
