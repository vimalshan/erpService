using AuditService.Application.DTOs;

namespace AuditService.Application.Interfaces;

public interface IJwtService
{
    AuthResponse GenerateToken(string username, string role, long empId);
    bool ValidateToken(string token);
}
