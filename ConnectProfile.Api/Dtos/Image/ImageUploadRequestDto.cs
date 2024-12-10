namespace ConnectProfile.Api.Dtos.Image;
public class ImageUploadRequestDto
{
    public IFormFile File { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
}
