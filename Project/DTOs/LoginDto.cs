using System.ComponentModel.DataAnnotations;

namespace auth_back.DTOs;

public class LoginDto
{
    [Required]
    public string ProgrammId { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}