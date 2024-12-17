using ConnectProfile.Api.Data;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectProfile.Api.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    public async Task AddOrUpdateImageAsync(Image image)
    {
        var existingImage = await context.Images.FirstOrDefaultAsync(i => i.AccountId == image.AccountId);

        if (existingImage != null)
        {
            existingImage.Name = image.Name;
            existingImage.Description = image.Description;
            existingImage.ImageBytes = image.ImageBytes;

            context.Images.Update(existingImage);
        }
        else
        {
            context.Images.Add(image);
        }

        await context.SaveChangesAsync();
    }

    public async Task<Image?> GetImageByAccountIdAsync(Guid accountId)
    {
        return await context.Images.FirstOrDefaultAsync(i => i.AccountId == accountId);
    }
}
