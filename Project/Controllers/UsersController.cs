using Microsoft.AspNetCore.Mvc;
using auth_back.DTOs;
using auth_back.Services;
using Microsoft.AspNetCore.Authorization;

namespace auth_back.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var userDto = await _userService.GetUserByIdAsync(id);

        if (userDto == null)
            return NotFound(new { Message = "User not found" });

        return Ok(userDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        try
        {
            var userDto = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updatedUserDto)
    {
        try
        {
            var userDto = await _userService.UpdateUserAsync(id, updatedUserDto);

            if (userDto == null)
                return NotFound(new { Message = "User not found" });

            return Ok(userDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDto>> DeleteUser(int id)
    {
        var userDto = await _userService.DeleteUserAsync(id);

        if (userDto == null)
            return NotFound(new { Message = "User not found" });

        return Ok(userDto);
    }
}