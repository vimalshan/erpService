using MobileAppManagement.Domain.Common;
using MobileAppManagement.Domain.Events;

namespace MobileAppManagement.Domain.Entities;

public class LoginDetail : AggregateRoot
{
    public decimal LoginId { get; private set; }
    public decimal UserSysId { get; private set; }
    public string? DeviceId { get; private set; }
    public DateTime Logon { get; private set; }
    public string Guid { get; private set; } = null!;
    public string? ImeiNo { get; private set; }
    public string? DeviceType { get; private set; }

    private LoginDetail() { }

    public static LoginDetail Create(decimal loginId, decimal userSysId, string? deviceId,
        string? imeiNo, string? deviceType)
    {
        var entity = new LoginDetail
        {
            LoginId = loginId,
            UserSysId = userSysId,
            DeviceId = deviceId,
            Logon = DateTime.UtcNow,
            Guid = System.Guid.NewGuid().ToString(),
            ImeiNo = imeiNo,
            DeviceType = deviceType
        };

        entity.AddDomainEvent(new UserLoggedInEvent(loginId, userSysId, deviceId ?? "", DateTime.UtcNow));
        return entity;
    }
}
