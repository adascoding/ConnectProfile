using ConnectProfile.Api.Data;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectProfile.Api.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    public async Task AddImageAsync(Image image)
    {
        context.Images.Add(image);
        await context.SaveChangesAsync();
    }

    public async Task<Image?> GetImageByAccountIdAsync(Guid accountId)
    {
        return await context.Images.FirstOrDefaultAsync(i => i.AccountId == accountId);
    }
}
