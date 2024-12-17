using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Mappers.Interfaces;

public interface IMapperService
{
    UserInfo MapToEntity(UserInfoCreateDto dto);
    UserInfoDto MapToDto(UserInfo entity);
}
