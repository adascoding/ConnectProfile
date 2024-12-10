using System.ComponentModel.DataAnnotations;

namespace ConnectProfile.Api.Entities;

public class Image
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public byte[] ImageBytes { get; set; } = null!;
    public Guid AccountId { get; set; }
}
