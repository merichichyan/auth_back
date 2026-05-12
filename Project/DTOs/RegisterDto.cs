using System.ComponentModel.DataAnnotations;

namespace auth_back.DTOs;

public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}