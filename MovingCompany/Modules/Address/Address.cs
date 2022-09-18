
using Dapper.Contrib.Extensions;

namespace MovingCompany.Models;

[Table("Address")]
public sealed class Address
{
    [Key]
    public int? ID { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string? Region { get; set; }
}
