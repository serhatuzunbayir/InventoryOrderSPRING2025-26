namespace Backend.Models;

public class Address
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AddressName { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}
