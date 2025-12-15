using SampleProjectBackEnd.Application.DTOs.Responses;

namespace SampleProjectBackEnd.Application.Interfaces.Services
{
    public interface ITokenService
    {
        TokenDto GenerateToken(int userId, string email, string userName, IList<string> roles);
        string GenerateRefreshToken();
        // ClaimsPrincipal GetPrincipalFromExpiredToken(string token); // Optional for refresh flow
    }
}
