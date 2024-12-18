using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Validators.Interfaces;
using System.Text.RegularExpressions;

namespace ConnectProfile.Api.Validators;

public class ValidationService(IAccountRepository accountRepository) : IValidationService
{
    public async Task<string?> ValidateRegisterAsync(RegisterRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName))
            return "Username is required.";

        if (!IsAlphabetical(dto.UserName))
            return "Username must contain only alphabetical characters.";

        if (string.IsNullOrWhiteSpace(dto.Email) || !IsValidEmail(dto.Email))
            return "A valid email is required.";

        if (string.IsNullOrWhiteSpace(dto.Password))
            return "Password is required.";

        if (!IsValidPassword(dto.Password))
            return "Password must be at least 8 characters long and contain a mix of uppercase, lowercase, digits, and special characters.";

        var existingUser = await accountRepository.GetByUserNameAsync(dto.UserName);
        if (existingUser != null)
            return "Username is already taken.";

        var existingEmail = await accountRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
            return "Email is already registered.";

        return null;
    }

    public string? ValidateLoginAsync(LoginRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName))
            return "Username is required.";

        if (string.IsNullOrWhiteSpace(dto.Password))
            return "Password is required.";

        return null;
    }

    public string? ValidateImageUpload(IFormFile file, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Image name is required.";

        if (string.IsNullOrWhiteSpace(description))
            return "Image description is required.";

        if (file == null || file.Length == 0)
            return "No file uploaded.";

        if (!IsValidImageFile(file))
            return "The file must be a JPG, JPEG, or PNG image.";

        if (file.Length > 10 * 1024 * 1024)
            return "The image file size must not exceed 10MB.";

        return null;
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
    private bool IsValidPassword(string password)
    {
        var passwordPattern = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
        return Regex.IsMatch(password, passwordPattern);
    }
    private bool IsValidImageFile(IFormFile file)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName)?.ToLower();

        return !string.IsNullOrEmpty(extension) && validExtensions.Contains(extension);
    }
    public string? ValidateUserInfo(UserInfoCreateDto dto)
    {
        var firstNameError = ValidateField("firstname", dto.FirstName);
        if (firstNameError != null)
            return firstNameError;

        var lastNameError = ValidateField("lastname", dto.LastName);
        if (lastNameError != null)
            return lastNameError;

        var emailError = ValidateField("email", dto.Email);
        if (emailError != null)
            return emailError;

        var phoneNumberError = ValidateField("phonenumber", dto.PhoneNumber);
        if (phoneNumberError != null)
            return phoneNumberError;

        if (dto.Address == null)
            return "Address is required.";

        var addressCityError = ValidateField("city", dto.Address.City);
        if (addressCityError != null)
            return addressCityError;

        var addressStreetError = ValidateField("street", dto.Address.Street);
        if (addressStreetError != null)
            return addressStreetError;

        var addressHouseNumberError = ValidateField("housenumber", dto.Address.HouseNumber);
        if (addressHouseNumberError != null)
            return addressHouseNumberError;

        var addressApartmentNumberError = ValidateField("apartmentnumber", dto.Address.ApartmentNumber);
        if (addressApartmentNumberError != null)
            return addressApartmentNumberError;

        return null;
    }

    public string? ValidateField(string fieldName, string fieldValue)
    {
        switch (fieldName.ToLower())
        {
            case "email":
                if (string.IsNullOrWhiteSpace(fieldValue) || !IsValidEmail(fieldValue))
                    return "Valid email is required.";
                break;

            case "phonenumber":
                if (string.IsNullOrWhiteSpace(fieldValue) || !IsValidPhoneNumber(fieldValue))
                    return "Phone number must be +3706 and include 7 additional digits.";
                break;

            case "firstname":
                if (string.IsNullOrWhiteSpace(fieldValue))
                    return "First name is required.";
                if (fieldValue.Trim().Length < 4)
                    return "First name must be at least 4 characters long.";
                break;

            case "lastname":
                if (string.IsNullOrWhiteSpace(fieldValue))
                    return "Last name is required.";
                if (fieldValue.Trim().Length < 4)
                    return "Last name must be at least 4 characters long.";
                break;

            case "city":
                if (string.IsNullOrWhiteSpace(fieldValue))
                    return "City is required.";
                if (fieldValue.Length > 100)
                    return "City cannot exceed 100 characters.";
                if (!IsAlphabetical(fieldValue))
                    return "City must contain only alphabetical symbols.";
                break;

            case "street":
                if (string.IsNullOrWhiteSpace(fieldValue) || fieldValue.Length > 100)
                    return "Street is required and cannot exceed 100 characters.";
                break;

            case "housenumber":
                if (string.IsNullOrWhiteSpace(fieldValue) || fieldValue.Length > 4)
                    return "House number is required and cannot exceed 4 characters.";
                break;

            case "apartmentnumber":
                if (string.IsNullOrWhiteSpace(fieldValue) || fieldValue?.Length > 3)
                    return "Apartment number is required and cannot exceed 3 characters.";
                break;

            default:
                return "Invalid field name.";
        }

        return null;
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^\+3706\d{7}$");
    }

    private bool IsAlphabetical(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z\s]+$");
    }

}
