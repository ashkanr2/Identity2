using AuthEduApi.Constants;
using AuthEduApi.DTOs;
using AuthEduApi.Models;
using AuthEduApi.Services.Jwt;
using Microsoft.AspNetCore.Identity;

namespace AuthEduApi.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AuthService(IJwtService jwtService , UserManager<AppUser> userManager, SignInManager<AppUser> signInManager , RoleManager<IdentityRole> roleManager)
        {
         _jwtService = jwtService;
         _userManager = userManager;
         _signInManager = signInManager;
         _roleManager = roleManager;

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
             var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail) 
                     ?? await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

            if(user is null)
                return new AuthResponseDto(false, null, "با اطلاعات ورودی  کاربری یافت نشد.", null);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if(!isPasswordValid)
                return new AuthResponseDto(false, null, "با اطلاعات ورودی  کاربری یافت نشد.", null);

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(user, roles);

            return new AuthResponseDto(true, token, $"کاربر {user.FullName} به سیستم خوش امدید .", user);

        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUserByUserName = await _userManager.FindByNameAsync(registerDto.UserName) is not null;
            var existingUserByEmail = await _userManager.FindByEmailAsync(registerDto.Email) is not null;


            if (existingUserByEmail || existingUserByUserName )
                return new AuthResponseDto(false, null, "با این اطلاعات کاربری ثبت شده است.", null);


            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                IsApproved=false,
                EmailConfirmed = true,
                IsPro = false,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return new AuthResponseDto(false, null, "خطا در ثبت نام کاربر.", null);

           

            var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);

            var roles = await _userManager.GetRolesAsync(user);


            var token = _jwtService.GenerateToken(user, roles);
            return new AuthResponseDto(true, token, $"کاربر {user.FullName} به سیستم خوش امدید .", user);

        }
    }
}
