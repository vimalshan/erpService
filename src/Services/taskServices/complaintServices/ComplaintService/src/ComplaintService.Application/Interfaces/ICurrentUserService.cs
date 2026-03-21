namespace ComplaintService.Application.Interfaces;

public interface ICurrentUserService
{
    decimal UserId { get; }
    string UserName { get; }
    bool IsAuthenticated { get; }
}
