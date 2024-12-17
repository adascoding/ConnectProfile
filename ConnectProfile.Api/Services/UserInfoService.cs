using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Mappers.Interfaces;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services.Interfaces;
using ConnectProfile.Api.Validators.Interfaces;

namespace ConnectProfile.Api.Services;

public class UserInfoService(
    IUserInfoRepository repository,
    IValidationService validationService,
    IMapperService mapper,
    ILogger<UserInfoService> logger) : IUserInfoService
{

    public async Task<Response<UserInfoDto>> GetUserInfoAsync(Guid accountId)
    {
        logger.LogInformation("Getting user info for AccountId: {AccountId}", accountId);

        var userInfo = await repository.GetByAccountIdAsync(accountId);

        if (userInfo == null)
        {
            logger.LogWarning("User info not found for AccountId: {AccountId}", accountId);
            return Response<UserInfoDto>.Fail("User info not found.");
        }

        var userInfoDto = mapper.MapToDto(userInfo);
        logger.LogInformation("User info retrieved successfully for AccountId: {AccountId}", accountId);
        return Response<UserInfoDto>.Ok(userInfoDto);
    }

    public async Task<Response<UserInfoDto>> CreateUserInfoAsync(UserInfoCreateDto userInfoCreateDto)
    {
        logger.LogInformation("Creating user info for AccountId: {AccountId}", userInfoCreateDto.AccountId);

        var validationError = validationService.ValidateUserInfo(userInfoCreateDto);
        if (validationError != null)
        {
            logger.LogWarning("Validation failed for AccountId: {AccountId}. Error: {Error}", userInfoCreateDto.AccountId, validationError);
            return Response<UserInfoDto>.Fail(validationError);
        }

        var userInfoEntity = mapper.MapToEntity(userInfoCreateDto);
        await repository.AddAsync(userInfoEntity);
        await repository.SaveChangesAsync();

        var userInfoDto = mapper.MapToDto(userInfoEntity);
        logger.LogInformation("User info successfully created for AccountId: {AccountId}", userInfoCreateDto.AccountId);
        return Response<UserInfoDto>.Ok(userInfoDto);
    }

    public async Task<Response<bool>> UpdateFieldAsync(Guid accountId, UpdateFieldRequestDto dto)
    {
        logger.LogInformation("Updating field {FieldName} for AccountId: {AccountId}", dto.FieldName, accountId);

        var userInfo = await repository.GetByAccountIdAsync(accountId);

        if (userInfo == null)
        {
            logger.LogWarning("User info not found for AccountId: {AccountId}", accountId);
            return Response<bool>.Fail("User info not found.");
        }

        var validationError = validationService.ValidateField(dto.FieldName, dto.FieldValue);
        if (validationError != null)
        {
            logger.LogWarning("Validation failed for field {FieldName} for AccountId: {AccountId}. Error: {Error}", dto.FieldName, accountId, validationError);
            return Response<bool>.Fail(validationError);
        }

        switch (dto.FieldName.ToLower())
        {
            case "firstname":
                userInfo.FirstName = dto.FieldValue;
                break;
            case "lastname":
                userInfo.LastName = dto.FieldValue;
                break;
            case "email":
                userInfo.Email = dto.FieldValue;
                break;
            case "personalcode":
                userInfo.PersonalCode = dto.FieldValue;
                break;
            case "phonenumber":
                userInfo.PhoneNumber = dto.FieldValue;
                break;
            case "city":
                userInfo.Address.City = dto.FieldValue;
                break;
            case "street":
                userInfo.Address.Street = dto.FieldValue;
                break;
            case "housenumber":
                userInfo.Address.HouseNumber = dto.FieldValue;
                break;
            case "apartmentnumber":
                userInfo.Address.ApartmentNumber = dto.FieldValue;
                break;
            default:
                logger.LogWarning("Invalid field name for AccountId: {AccountId}. Field: {FieldName}", accountId, dto.FieldName);
                return Response<bool>.Fail("Invalid field name.");
        }

        await repository.SaveChangesAsync();
        logger.LogInformation("Field {FieldName} successfully updated for AccountId: {AccountId}", dto.FieldName, accountId);
        return Response<bool>.Ok(true);
    }
}
