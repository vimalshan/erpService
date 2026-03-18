namespace ReferenceService.Domain;

/// <summary>
/// Represents status of an entity (Active/Inactive).
/// </summary>
public enum EntityStatus
{
    Active = 1,
    Inactive = 0
}

public static class EntityStatusExtensions
{
    public static char ToChar(this EntityStatus status) => status == EntityStatus.Active ? 'Y' : 'N';
    
    public static EntityStatus FromChar(char status) => status == 'Y' ? EntityStatus.Active : EntityStatus.Inactive;
}
