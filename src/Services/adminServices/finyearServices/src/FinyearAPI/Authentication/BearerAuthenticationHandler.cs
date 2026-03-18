using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.AuthProvider.Authentication;
using System.Text.Encodings.Web;

namespace FinyearAPI.Authentication
{
    /// <summary>
    /// Custom Bearer token authentication handler
    /// Validates JWT tokens using the AuthService
    /// </summary>
    public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthService _authService;

        public BearerAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthService authService)
            : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var principal = _authService.ValidateToken(token);
                if (principal == null)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid token"));

                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error validating token");
                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
            }
        }
    }
}
