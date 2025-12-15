using Microsoft.AspNetCore.Mvc;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.Interfaces.Services;

namespace SampleProjectBackEnd.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result.Success && result.Data != null)
            {
                SetRefreshTokenCookie(result.Data.RefreshToken);
                // Access Token'ı body ile dön, Refresh Token'ı gizle
                return Ok(new SuccessDataResult<object>(new { AccessToken = result.Data.AccessToken, Expiration = result.Data.Expiration }, result.Message)); 
            }

            return Unauthorized(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new ErrorResult("Refresh Token bulunamadı."));

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (result.Success && result.Data != null)
            {
                SetRefreshTokenCookie(result.Data.RefreshToken);
                return Ok(new SuccessDataResult<object>(new { AccessToken = result.Data.AccessToken, Expiration = result.Data.Expiration }, result.Message));
            }

            return Unauthorized(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeRefreshTokenAsync(refreshToken);
            }

            Response.Cookies.Delete("refreshToken");
            return Ok(new SuccessResult("Başarıyla çıkış yapıldı."));
        }

        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Strict,
                Secure = false   // Production'da true olmalı
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
