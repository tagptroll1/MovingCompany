using Dapper.Contrib.Extensions;

namespace MovingCompany.Models;

[Table("Phone")]
public sealed class Phone
{
    public int ID { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CountryCode { get; set; }
}
