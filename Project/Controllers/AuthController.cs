using Microsoft.AspNetCore.Mvc;
using auth_back.DTOs;
using auth_back.Services;

namespace auth_back.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var userDto = await _authService.RegisterAsync(dto);
            return Ok(userDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
            return Unauthorized(new { Message = "Invalid username or password" });

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto);

        if (result == null)
            return Unauthorized(new { Message = "Invalid or expired refresh token" });

        return Ok(result);
    }
}
