namespace MenuAndSecurityService.Application.DTOs;

public class AuthTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string UserName { get; set; } = string.Empty;
}
