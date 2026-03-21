namespace SecurityService.Functions.BackgroundTasks;

public class UserCleanupWorker : BackgroundService
{
    private readonly ILogger<UserCleanupWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UserCleanupWorker(ILogger<UserCleanupWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("UserCleanupWorker running at: {Time}", DateTimeOffset.Now);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<SecurityService.Domain.Interfaces.IUnitOfWork>();

                // Cleanup: deactivate users who haven't logged in for 90 days
                var users = await uow.Users.GetAllAsync(stoppingToken);
                var cutoff = DateTime.UtcNow.AddDays(-90);

                foreach (var user in users.Where(u => u.IsActive && u.LastLogin.HasValue && u.LastLogin < cutoff))
                {
                    user.IsActive = false;
                    await uow.Users.UpdateAsync(user, stoppingToken);
                    _logger.LogInformation("Deactivated inactive user: {Username}", user.Username);
                }

                await uow.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UserCleanupWorker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
