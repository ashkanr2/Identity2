using AuthEduApi.DTOs;

namespace AuthEduApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto );
    }
}
