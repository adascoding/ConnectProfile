using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services.Interfaces;
using ConnectProfile.Api.Validators.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ConnectProfile.Api.Services;

public class ImageService(
    IImageRepository imageRepository,
    IValidationService validationService,
    ILogger<ImageService> logger
    ) : IImageService
{
    public async Task<Response<object>> UploadImageAsync(Guid accountId, IFormFile file, string name, string description)
    {
        logger.LogInformation("Starting image upload process for account ID {AccountId}", accountId);

        var validationResult = validationService.ValidateImageUpload(file, name, description);
        if (validationResult != null)
        {
            logger.LogWarning("Image upload validation failed: {Error}", validationResult);
            return Response<object>.Fail(validationResult);
        }

        try
        {
            logger.LogInformation("Reading and processing the uploaded image file.");
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            var resizedImage = ResizeImage(imageBytes, 200, 200);
            logger.LogInformation("Image resized successfully for account ID {AccountId}", accountId);

            var imageEntity = new Entities.Image
            {
                Name = name,
                Description = description,
                ImageBytes = resizedImage,
                AccountId = accountId
            };

            await imageRepository.AddOrUpdateImageAsync(imageEntity);
            logger.LogInformation("Image uploaded successfully for account ID {AccountId}", accountId);

            return Response<object>.Ok(new { message = "Image uploaded successfully", accountId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while uploading the image for account ID {AccountId}", accountId);
            return Response<object>.Fail($"Error uploading image: {ex.Message}");
        }
    }

    public async Task<Entities.Image?> GetImageByAccountIdAsync(Guid accountId)
    {
        logger.LogInformation("Fetching image for account ID {AccountId}", accountId);

        var imageEntity = await imageRepository.GetImageByAccountIdAsync(accountId);
        if (imageEntity == null)
        {
            logger.LogWarning("No image found for account ID {AccountId}", accountId);
        }
        else
        {
            logger.LogInformation("Image retrieved successfully for account ID {AccountId}", accountId);
        }

        return imageEntity;
    }

    public byte[] ResizeImage(byte[] imageBytes, int width, int height)
    {
        using var imageStream = new MemoryStream(imageBytes);
        using var image = SixLabors.ImageSharp.Image.Load(imageStream);
        image.Mutate(x => x.Resize(width, height));

        using var memoryStream = new MemoryStream();
        image.SaveAsJpeg(memoryStream);
        return memoryStream.ToArray();
    }
}
