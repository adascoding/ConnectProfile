namespace ConnectProfile.Api.Dtos.UserInfo;

public class UserInfoCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public AddressDto Address { get; set; } = new AddressDto();
}

public class AddressDto
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
}
