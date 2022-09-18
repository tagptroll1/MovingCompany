using Dapper.Contrib.Extensions;

namespace MovingCompany.Models;

[Table("Customer")]
public sealed class Customer
{
    [Key]
    public int? ID { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? PhoneID { get; set; }
    [Computed]
    public Phone? Phone { get; set; }
    public bool Deleted { get; set; }
}
