using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Domain.Events;
using ApprovalGroup.Domain.Exceptions;

namespace ApprovalGroup.Domain.Aggregates;

/// <summary>
/// ApprovalGroupAggregate - Aggregate root that combines ApprovalGroupMaster with its mappings
/// </summary>
public class ApprovalGroupAggregate
{
    public ApprovalGroupMaster Group { get; private set; }
    public IReadOnlyList<ApprovalGroupMap> Maps => _maps.AsReadOnly();
    public IReadOnlyList<ApprovalGroupUserMap> UserMaps => _userMaps.AsReadOnly();

    private readonly List<ApprovalGroupMap> _maps = new();
    private readonly List<ApprovalGroupUserMap> _userMaps = new();

    public ApprovalGroupAggregate(ApprovalGroupMaster group)
    {
        Group = group;
        _maps.AddRange(group.GroupMaps);
        _userMaps.AddRange(group.UserMaps);
    }

    public ApprovalGroupMap AddMap(long mapId, int payBySpecific, int buSpecific,
        long mainCat, long subCat, long createdBy, char? currency = null)
    {
        var map = ApprovalGroupMap.Create(mapId, Group.GroupId, payBySpecific, buSpecific,
            mainCat, subCat, createdBy, currency);
        _maps.Add(map);
        return map;
    }

    public ApprovalGroupUserMap AddUserMap(long mapId, long userId, DateTime effectiveDate, long createdBy)
    {
        var existing = _userMaps.FirstOrDefault(um => um.MapUserId == userId && um.MapClosureDate == null);
        if (existing is not null)
            throw new InvalidOperationException($"User {userId} is already actively mapped to this group.");

        var userMap = ApprovalGroupUserMap.Create(mapId, Group.GroupId, userId, effectiveDate, createdBy);
        _userMaps.Add(userMap);
        return userMap;
    }

    public void RemoveUserMap(long userId, long modifiedBy)
    {
        var userMap = _userMaps.FirstOrDefault(um => um.MapUserId == userId && um.MapClosureDate == null)
            ?? throw new UserMapNotFoundException(userId);
        userMap.Close(modifiedBy);
    }
}
