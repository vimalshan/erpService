using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanFestival : AggregateRoot<long>
{
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private readonly List<LoanFestivalMap> _festivalMaps = [];
    public IReadOnlyCollection<LoanFestivalMap> FestivalMaps => _festivalMaps.AsReadOnly();

    private LoanFestival() { }

    public static LoanFestival Create(long id, string description, DateTime startDate, DateTime endDate, long modifiedBy)
    {
        return new LoanFestival
        {
            Id = id,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            CreatedBy = modifiedBy,
            CreatedOn = DateTime.UtcNow,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string description, DateTime startDate, DateTime endDate, long modifiedBy)
    {
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
