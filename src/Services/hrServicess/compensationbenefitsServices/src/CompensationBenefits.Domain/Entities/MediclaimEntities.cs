using CompensationBenefits.Domain.Common;
using CompensationBenefits.Domain.Events;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to MEDICLAIM_MASTER table</summary>
public class MediclaimMaster : BaseEntity
{
    public long MediclaimId { get; private set; }
    public string? MediclaimRefName { get; private set; }
    public long? MediclaimProviderId { get; private set; }
    public long? MediclaimTppId { get; private set; }
    public DateTime? MediclaimStartDate { get; private set; }
    public DateTime? MediclaimCloseDate { get; private set; }
    public DateTime? MediclaimMaxEntryDate { get; private set; }
    public string? MediclaimInsRefNo { get; private set; }
    public string? MediclaimType { get; private set; }    // I=Individual, F=Family
    public string? MediclaimPaidBy { get; private set; } // E=Employee, C=Company, U=UpToLevel
    public long? MediclaimServiceTaxPer { get; private set; }
    public decimal? MediclaimCompPayLimit { get; private set; }
    public long? MediclaimLoadingPer { get; private set; }
    public long? MediclaimNonClaimPer { get; private set; }

    public ICollection<MediclaimDetail> Details { get; private set; } = [];
    public ICollection<MediclaimYearlyPremium> YearlyPremiums { get; private set; } = [];

    private MediclaimMaster() { }

    public static MediclaimMaster Create(long id, string refName, string type, string paidBy,
        DateTime startDate, DateTime closeDate)
    {
        var m = new MediclaimMaster
        {
            MediclaimId = id,
            MediclaimRefName = refName,
            MediclaimType = type,
            MediclaimPaidBy = paidBy,
            MediclaimStartDate = startDate,
            MediclaimCloseDate = closeDate
        };
        m.AddDomainEvent(new MediclaimUpdatedDomainEvent(id, refName));
        return m;
    }
}

/// <summary>Maps to MEDICLAIM_DET table</summary>
public class MediclaimDetail : BaseEntity
{
    public long MedNominationRunId { get; private set; }
    public long MedNominationId { get; private set; }
    public long MedRelationship { get; private set; }
    public string MedNomineeName { get; private set; } = default!;
    public DateTime? MedNomineeDob { get; private set; }
    public decimal? MedNomineeAge { get; private set; }
    public string MedNomineeGender { get; private set; } = default!;
    public decimal? MedPremium { get; private set; }
    public string MedTaxStatus { get; private set; } = default!;
    public long? MedNetPremium { get; private set; }
    public long? MedPremiumServiceTax { get; private set; }
    public long? MedGrossPremium { get; private set; }

    private MediclaimDetail() { }
}

/// <summary>Maps to MEDICLAIM_EXCEPTION table</summary>
public class MediclaimException : BaseEntity
{
    public long MediclaimEmpSysId { get; private set; }
    public long MediclaimId { get; private set; }

    private MediclaimException() { }
}

/// <summary>Maps to MEDICLAIM_PREMPERCENTAGE table</summary>
public class MediclaimPremiumPercentage : BaseEntity
{
    public long MedPpId { get; private set; }
    public long MedRelationshipId { get; private set; }
    public decimal MedPercentage { get; private set; }

    private MediclaimPremiumPercentage() { }
}

/// <summary>Maps to MEDICLAIM_YEARLYPREM table</summary>
public class MediclaimYearlyPremium : BaseEntity
{
    public long MedYpYearlyPremId { get; private set; }
    public long MedYpMediclaimId { get; private set; }
    public decimal MedYpSumAssured { get; private set; }
    public decimal MedYpPremiumAmnt { get; private set; }
    public long MedYpModifiedBy { get; private set; }
    public DateTime MedYpModifiedOn { get; private set; }
    public string MedYpType { get; private set; } = default!; // R=Regular, T=TopUp

    public MediclaimMaster MediclaimMaster { get; private set; } = default!;

    private MediclaimYearlyPremium() { }
}
