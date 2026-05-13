using auth_back.DTOs;

namespace auth_back.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponse?> LoginAsync(LoginDto loginDto);
}
