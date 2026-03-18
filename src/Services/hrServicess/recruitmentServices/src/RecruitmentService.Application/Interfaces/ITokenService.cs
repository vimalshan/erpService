namespace RecruitmentService.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(decimal userId, string email, IEnumerable<string> roles);
    (decimal userId, string email) ValidateToken(string token);
}
