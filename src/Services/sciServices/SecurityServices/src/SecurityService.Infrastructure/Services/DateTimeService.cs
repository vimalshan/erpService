using SecurityService.Application.Interfaces;

namespace SecurityService.Infrastructure.Services;

public sealed class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
