using auth_back.DTOs;

namespace auth_back.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<UserDto?> DeleteUserAsync(int id);
}
