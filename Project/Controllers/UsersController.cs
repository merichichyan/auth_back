using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auth_back.Data;
using auth_back.Models;

namespace auth_back.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return Ok(users);
    }


[HttpGet("{id}")]
public async Task<IActionResult> GetUserById(int id)
{
    var user = await _context.Users.FindAsync(id);

    if (user == null)
        return NotFound(new { Message = "User not found" });

    return Ok(user);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser(int id, User updatedUser)
{
    var user = await _context.Users.FindAsync(id);

    if (user == null)
        return NotFound(new { Message = "User not found" });

    user.Username = updatedUser.Username;
    user.Email = updatedUser.Email;

    await _context.SaveChangesAsync();

    return Ok(new { Message = "User updated successfully" });
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    var user = await _context.Users.FindAsync(id);

    if (user == null)
        return NotFound(new { Message = "User not found" });

    _context.Users.Remove(user);

    await _context.SaveChangesAsync();

    return Ok(new { Message = "User deleted successfully" });
}
}