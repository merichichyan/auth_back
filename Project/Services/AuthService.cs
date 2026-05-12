using Microsoft.EntityFrameworkCore;
using auth_back.Data;
using auth_back.DTOs;
using auth_back.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth_back.Services;

public class AuthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
}

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // REGISTER
    public async Task<AuthResponse> Register(RegisterDto dto)
    {
        var userExists = await _context.Users.AnyAsync(x => x.Email == dto.Email);
        if (userExists)
            return new AuthResponse { Success = false, Message = "User already exists" };

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse { Success = true, Message = "User created successfully" };
    }

    // LOGIN
    public async Task<LoginResponse?> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var token = CreateToken(user);

        return new LoginResponse { Token = token };
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds,
            Issuer = _configuration.GetSection("Jwt:Issuer").Value,
            Audience = _configuration.GetSection("Jwt:Audience").Value
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}