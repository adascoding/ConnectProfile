using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Repositories.Interfaces;

public interface IImageRepository
{
    Task AddImageAsync(Image image);
    Task<Image?> GetImageByAccountIdAsync(Guid accountId);
}
