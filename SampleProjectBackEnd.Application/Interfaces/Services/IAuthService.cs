using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;

namespace SampleProjectBackEnd.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(UserRegisterRequest request);
        Task<IDataResult<TokenDto>> LoginAsync(UserLoginRequest request);
        Task<IDataResult<TokenDto>> RefreshTokenAsync(string refreshToken);
        Task<IResult> RevokeRefreshTokenAsync(string refreshToken);
    }
}
