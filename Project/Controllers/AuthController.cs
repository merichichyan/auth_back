using Microsoft.AspNetCore.Mvc;
using auth_back.DTOs;
using auth_back.Services;

namespace auth_back.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.Register(dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.Login(dto);

        if (result == null)
            return Unauthorized(new { Message = "Invalid username or password" });

        return Ok(result);
    }
}
