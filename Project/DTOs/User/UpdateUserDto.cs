using System.ComponentModel.DataAnnotations;

namespace auth_back.DTOs;

public class UpdateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}