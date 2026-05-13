using AutoMapper;
using Microsoft.EntityFrameworkCore;
using auth_back.Data;
using auth_back.DTOs;
using auth_back.Models;

namespace auth_back.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Business rule: Check if username or email already exists
        if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email || u.Username == createUserDto.Username))
        {
            throw new InvalidOperationException("User with the same email or username already exists.");
        }

        var user = _mapper.Map<User>(createUserDto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        // Business rule: Check if email is being taken by someone else
        if (await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.Id != id))
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        _mapper.Map(updateUserDto, user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }
}
