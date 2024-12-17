using System.ComponentModel.DataAnnotations;

namespace ConnectProfile.Api.Entities;

public class Address
{
    [Required]
    [MaxLength(100)]
    public string City { get; set; }

    [Required]
    [MaxLength(200)]
    public string Street { get; set; }

    [Required]
    [MaxLength(20)]
    public string HouseNumber { get; set; }

    [MaxLength(10)]
    public string ApartmentNumber { get; set; }
}
