using ConnectProfile.Api.Repositories;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ConnectProfile.Api.Services;

public class ImageService(IImageRepository imageRepository) : IImageService
{
    public async Task AddImageAsync(Entities.Image image)
    {
        using var imageStream = new MemoryStream(image.ImageBytes);
        var resizedImage = ResizeImage(imageStream, 200, 200);

        image.ImageBytes = resizedImage;

        await imageRepository.AddImageAsync(image);
    }

    public async Task<Entities.Image?> GetImageByAccountIdAsync(Guid accountId)
    {
        return await imageRepository.GetImageByAccountIdAsync(accountId);
    }

    private byte[] ResizeImage(Stream imageStream, int width, int height)
    {
        using var image = SixLabors.ImageSharp.Image.Load(imageStream);
        image.Mutate(x => x.Resize(width, height));

        using var memoryStream = new MemoryStream();
        image.SaveAsJpeg(memoryStream);
        return memoryStream.ToArray();
    }
}
