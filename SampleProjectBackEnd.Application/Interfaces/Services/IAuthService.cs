using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;

namespace SampleProjectBackEnd.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IResult> RegisterAsync(UserRegisterRequest request);
        Task<IDataResult<string>> LoginAsync(UserLoginRequest request);
    }
}
