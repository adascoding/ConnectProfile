using System.ComponentModel.DataAnnotations;

namespace ConnectProfile.Api.Entities;

public class Image
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public byte[] ImageBytes { get; set; }

    [Required]
    public Guid AccountId { get; set; }
}
