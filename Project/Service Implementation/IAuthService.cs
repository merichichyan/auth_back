using auth_back.DTOs;

namespace auth_back.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
}
