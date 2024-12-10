using System.ComponentModel.DataAnnotations;

namespace ConnectProfile.Api.Entities;

public class Account
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public byte[] PasswordHash { get; set; } = null!;

    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    [Required]
    public string Role { get; set; } = "User";
}