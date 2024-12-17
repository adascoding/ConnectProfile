using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Mappers.Interfaces;

namespace ConnectProfile.Api.Mappers;

public class MapperService : IMapperService
{
    public UserInfo MapToEntity(UserInfoCreateDto dto)
    {
        return new UserInfo
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PersonalCode = dto.PersonalCode,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            AccountId = dto.AccountId,
            Address = new Address
            {
                City = dto.Address.City,
                Street = dto.Address.Street,
                HouseNumber = dto.Address.HouseNumber,
                ApartmentNumber = dto.Address.ApartmentNumber
            }
        };
    }

    public UserInfoDto MapToDto(UserInfo entity)
    {
        return new UserInfoDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            PersonalCode = entity.PersonalCode,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            Address = new AddressDto
            {
                City = entity.Address.City,
                Street = entity.Address.Street,
                HouseNumber = entity.Address.HouseNumber,
                ApartmentNumber = entity.Address.ApartmentNumber
            }
        };
    }
}
