namespace TravelService.Application.Common.Interfaces;

public interface ICurrentUser
{
    string UserId { get; }
    string UserName { get; }
    string Email { get; }
    IEnumerable<string> Roles { get; }
    bool IsAuthenticated { get; }
}
