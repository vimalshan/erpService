namespace Document.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    decimal? EmployeePin { get; }
    bool IsAuthenticated { get; }
}
