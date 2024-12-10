using ConnectProfile.Api.Data;
using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConnectProfile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserInfoController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("{accountId}")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo(Guid accountId)
    {
        var userInfo = await context.UserInfos
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.AccountId == accountId);

        if (userInfo == null)
            return NotFound();

        var userInfoDto = new UserInfoDto
        {
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            PersonalCode = userInfo.PersonalCode,
            PhoneNumber = userInfo.PhoneNumber,
            Email = userInfo.Email,
            Address = new AddressDto
            {
                City = userInfo.Address.City,
                Street = userInfo.Address.Street,
                HouseNumber = userInfo.Address.HouseNumber,
                ApartmentNumber = userInfo.Address.ApartmentNumber
            }
        };

        return Ok(userInfoDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserInfo>> CreateUserInfo(UserInfoCreateDto userInfoCreateDto)
    {
        try
        {
            var userInfo = new UserInfo
            {
                Id = Guid.NewGuid(),
                FirstName = userInfoCreateDto.FirstName,
                LastName = userInfoCreateDto.LastName,
                PersonalCode = userInfoCreateDto.PersonalCode,
                PhoneNumber = userInfoCreateDto.PhoneNumber,
                Email = userInfoCreateDto.Email,
                AccountId = userInfoCreateDto.AccountId,
                Address = new Address
                {
                    City = userInfoCreateDto.Address.City,
                    Street = userInfoCreateDto.Address.Street,
                    HouseNumber = userInfoCreateDto.Address.HouseNumber,
                    ApartmentNumber = userInfoCreateDto.Address.ApartmentNumber
                }
            };

            context.UserInfos.Add(userInfo);
            await context.SaveChangesAsync();

            var userInfoDto = new UserInfoDto
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PersonalCode = userInfo.PersonalCode,
                PhoneNumber = userInfo.PhoneNumber,
                Email = userInfo.Email,
                Address = new AddressDto
                {
                    City = userInfo.Address.City,
                    Street = userInfo.Address.Street,
                    HouseNumber = userInfo.Address.HouseNumber,
                    ApartmentNumber = userInfo.Address.ApartmentNumber
                }
            };

            return CreatedAtAction(nameof(GetUserInfo), new { accountId = userInfo.AccountId }, userInfoDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An error occurred while saving the user info.");
        }
    }

    [HttpPatch("{accountId}/FirstName")]
    public async Task<IActionResult> UpdateFirstName(Guid accountId, [FromBody] string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return BadRequest("First name cannot be empty.");

        var userInfo = await context.UserInfos.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null)
            return NotFound();

        userInfo.FirstName = firstName;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/LastName")]
    public async Task<IActionResult> UpdateLastName(Guid accountId, [FromBody] string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return BadRequest("Last name cannot be empty.");

        var userInfo = await context.UserInfos.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null)
            return NotFound();

        userInfo.LastName = lastName;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/PersonalCode")]
    public async Task<IActionResult> UpdatePersonalCode(Guid accountId, [FromBody] string personalCode)
    {
        if (string.IsNullOrWhiteSpace(personalCode))
            return BadRequest("Personal code cannot be empty.");

        var userInfo = await context.UserInfos.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null)
            return NotFound();

        userInfo.PersonalCode = personalCode;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/PhoneNumber")]
    public async Task<IActionResult> UpdatePhoneNumber(Guid accountId, [FromBody] string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return BadRequest("Phone number cannot be empty.");

        var userInfo = await context.UserInfos.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null)
            return NotFound();

        userInfo.PhoneNumber = phoneNumber;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/Email")]
    public async Task<IActionResult> UpdateEmail(Guid accountId, [FromBody] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email cannot be empty.");

        var userInfo = await context.UserInfos.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null)
            return NotFound();

        userInfo.Email = email;
        await context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPatch("{accountId}/City")]
    public async Task<IActionResult> UpdateCity(Guid accountId, [FromBody] string city)
    {
        var userInfo = await context.UserInfos.Include(u => u.Address).FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null || userInfo.Address == null)
            return NotFound();

        userInfo.Address.City = city;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/Street")]
    public async Task<IActionResult> UpdateStreet(Guid accountId, [FromBody] string street)
    {
        var userInfo = await context.UserInfos.Include(u => u.Address).FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null || userInfo.Address == null)
            return NotFound();

        userInfo.Address.Street = street;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/HouseNumber")]
    public async Task<IActionResult> UpdateHouseNumber(Guid accountId, [FromBody] string houseNumber)
    {
        var userInfo = await context.UserInfos.Include(u => u.Address).FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null || userInfo.Address == null)
            return NotFound();

        userInfo.Address.HouseNumber = houseNumber;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{accountId}/ApartmentNumber")]
    public async Task<IActionResult> UpdateApartmentNumber(Guid accountId, [FromBody] string apartmentNumber)
    {
        var userInfo = await context.UserInfos.Include(u => u.Address).FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (userInfo == null || userInfo.Address == null)
            return NotFound();

        userInfo.Address.ApartmentNumber = apartmentNumber;
        await context.SaveChangesAsync();

        return NoContent();
    }

}
