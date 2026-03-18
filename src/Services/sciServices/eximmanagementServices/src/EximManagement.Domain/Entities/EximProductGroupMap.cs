using EximManagement.Domain.Common;

namespace EximManagement.Domain.Entities;

public class EximProductGroupMap : BaseEntity
{
    public long MapId { get; private set; }
    public long GroupId { get; private set; }
    public long ProductId { get; private set; }
    public long LastUpdatedBy { get; private set; }
    public DateTime LastUpdatedOn { get; private set; }

    private EximProductGroupMap() { }

    public static EximProductGroupMap Create(long mapId, long groupId, long productId, long updatedBy)
        => new() { MapId = mapId, GroupId = groupId, ProductId = productId, LastUpdatedBy = updatedBy, LastUpdatedOn = DateTime.UtcNow };
}
