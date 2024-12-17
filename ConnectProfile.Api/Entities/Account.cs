using System.ComponentModel.DataAnnotations;

namespace ConnectProfile.Api.Entities;

public class Account
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    [Required]
    public string Role { get; set; } = "User";
}
