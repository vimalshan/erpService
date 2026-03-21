using MediatR;

namespace AdminService.Domain.Events;

public record AdminMasterCreatedEvent(string AdminId, string AdminName) : INotification;
public record AdminMasterUpdatedEvent(string AdminId, string AdminName) : INotification;
public record AdminUserMapCreatedEvent(string MapId, string AdminId, string EmpSysId) : INotification;
public record AdminUserMapUpdatedEvent(string MapId, string AdminId) : INotification;
public record AdminFinUserMapCreatedEvent(string FinanceMapId, string FinanceEmpSysId) : INotification;
public record AccessRightsGrantedEvent(string RightsId, string UserId, string RightsFor) : INotification;
public record AccessRightsRevokedEvent(string RightsId, string UserId) : INotification;
