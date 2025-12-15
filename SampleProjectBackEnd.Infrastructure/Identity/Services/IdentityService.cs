using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.DTOs.Responses;
using SampleProjectBackEnd.Application.Interfaces.Services;
using SampleProjectBackEnd.Domain.Entities;
using SampleProjectBackEnd.Infrastructure.Persistence;

namespace SampleProjectBackEnd.Infrastructure.Identity.Services
{
    public class IdentityService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<IResult> RegisterAsync(UserRegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return new ErrorResult("Bu email zaten kayıtlı.");

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(" | ", createResult.Errors.Select(e => e.Description));
                return new ErrorResult(errors);
            }

            return new SuccessResult("Kullanıcı başarıyla oluşturuldu.");
        }

        public async Task<IDataResult<TokenDto>> LoginAsync(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new ErrorDataResult<TokenDto>("Kullanıcı bulunamadı.");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
                return new ErrorDataResult<TokenDto>("Email veya şifre hatalı.");

            return await CreateTokenForUser(user);
        }

        public async Task<IDataResult<TokenDto>> RefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);

            if (refreshToken == null)
                return new ErrorDataResult<TokenDto>("Geçersiz Refresh Token.");

            if (!refreshToken.IsActive)
                return new ErrorDataResult<TokenDto>("Refresh Token süresi dolmuş veya iptal edilmiş.");

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user == null)
                return new ErrorDataResult<TokenDto>("Kullanıcı bulunamadı.");

            var newTokens = await CreateTokenForUser(user);

            // Revoke old token
            refreshToken.Revoke(newTokens.Data.RefreshToken);
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return newTokens;
        }

        private async Task<IDataResult<TokenDto>> CreateTokenForUser(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var tokenDto = _tokenService.GenerateToken(user.Id, user.Email!, user.UserName!, roles);

            var refreshToken = new RefreshToken(
                user.Id,
                tokenDto.RefreshToken,
                DateTime.UtcNow.AddDays(7) // 7 gün geçerli
            );

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new SuccessDataResult<TokenDto>(tokenDto, "Giriş başarılı.");
        }

        public async Task<IResult> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);

            if (refreshToken == null)
                return new ErrorResult("Token bulunamadı.");

            refreshToken.Revoke();
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return new SuccessResult("Token iptal edildi.");
        }
    }
}
