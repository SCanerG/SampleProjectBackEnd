using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Infrastructure.Identity; // Needed for ApplicationUser reference if we pass it directly, OR we can use claims. Better to try keeping Infra dependency out of Application if possible, but ApplicationUser is often shared. Actually Application usually doesn't depend on Infra. We should use an interface or a DTO here. But for now, let's keep it simple as user provided ApplicationUser in Infra. Wait, Application -> Infra reference is WRONG. Infra -> Application is correct. 
// So Application cannot see ApplicationUser if it's in Infra.
// I check ApplicationUser location: c:\... \Infrastructure\Identity\ApplicationUser.cs
// Application CANNOT depend on Infrastructure.
// So ITokenService cannot take ApplicationUser as parameter if it is in Application layer.
// I must use a DTO or a Domain Entity (if User was in Domain).
// Current User is in Infra.
// I will change ITokenService to take (int userId, string email, IList<string> roles) or similar primitive types.

namespace SampleProjectBackEnd.Infrastructure.Token
{
    // Wait, ITokenService is likely in Application currently?
    // Let me check.
}
