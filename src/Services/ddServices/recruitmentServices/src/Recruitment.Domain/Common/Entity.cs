namespace Recruitment.Domain.Common;

/// <summary>
/// Base entity class with common properties
/// </summary>
public abstract class Entity
{
    public decimal Id { get; protected set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    protected Entity()
    {
        CreatedDate = DateTime.UtcNow;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Entity entity)
            return false;

        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
