using Dapper.Contrib.Extensions;

namespace MovingCompany.Models;

[Table("MovingOrder")]
public sealed class MovingOrder
{
    public int ID { get; set; }
    public int? CustomerID { get; set; }
    [Computed]
    public Customer? Customer { get; set; }
    public int? MoveFromAddressID { get; set; }
    [Computed]
    public Address? MoveFrom { get; set; }
    public int? MoveToAddressID { get; set; }
    [Computed]
    public Address? MoveTo { get; set; }
    public MovingServiceEnum Service { get; set; }
    public DateTime MoveFromDate { get; set; }
    public DateTime MoveToDate { get; set; }
    public DateTime PackingDate { get; set; }
    public DateTime CleaningDate { get; set; }
    public string? Comment { get; set; }
    public MovingStatusCode StatusCode { get; set; }
    public bool Deleted { get; set; }
}

// Here it is assumed packing and cleaning is for the moving from address,
// More enums can be added for services for moving to address.
[Flags] // Combine for multiple services
public enum MovingServiceEnum : short
{
    None = 0,
    Moving = 1,
    Packing = 2,
    Cleaning = 4,
}

public enum MovingStatusCode
{
    Created = 10001,
    Processing = 20001,
    Completed = 30001,
}
