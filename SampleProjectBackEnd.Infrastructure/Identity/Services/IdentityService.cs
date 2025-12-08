using Microsoft.AspNetCore.Identity;
using SampleProjectBackEnd.Application.Common.Results;
using SampleProjectBackEnd.Application.DTOs.Requests;
using SampleProjectBackEnd.Application.Interfaces.Services;
using SampleProjectBackEnd.Domain.Entities;
using SampleProjectBackEnd.Infrastructure.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProjectBackEnd.Infrastructure.Identity.Services
{
    public class IdentityService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public IdentityService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            JwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<IResult> RegisterAsync(UserRegisterRequest request)
        {
            // 1 - E-posta var mi kontrol et
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new ErrorResult("Bu email zaten kayitli.");
            }

            // 2 - Kullanici olustur
            var user = new AppUser
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            // 3 - Hata varsa detaylarini dondur
            if (!createResult.Succeeded)
            {
                var errors = string.Join(" | ", createResult.Errors.Select(e => e.Description));
                return new ErrorResult(errors);
            }

            // 4 - (Opsiyonel) rol atama, onay maili vs. buraya gelir

            return new SuccessResult("Kullanici basariyla olusturuldu.");
        }

        public async Task<IDataResult<string>> LoginAsync(UserLoginRequest request)
        {
            // 1 - Kullanici var mi
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ErrorDataResult<string>("Kullanici bulunamadi.");
            }

            // 2 - Sifre dogru mu
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                return new ErrorDataResult<string>("Email veya sifre hatali.");
            }

            // 3 - Rollerini al
            var roles = await _userManager.GetRolesAsync(user);

            // 4 - Token olustur
            var token = _jwtTokenHelper.GenerateToken(user, roles);

            return new SuccessDataResult<string>(token, "Giris basarili.");
        }
    }
}
