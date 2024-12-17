using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectProfile.Api.Entities;

public class UserInfo
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(20)]
    public string PersonalCode { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [ForeignKey(nameof(Image))]
    public Guid? ProfilePictureId { get; set; }
    public Image? ProfilePicture { get; set; }

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    [Required]
    public Address Address { get; set; }
}
