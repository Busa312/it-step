using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models;

public class Car
{
    [Key]
    public int Id { get; set; }
    public string Brand { get; set; }
    public int Year { get; set; }
    public int PriceDay { get; set; }
    public int Capacity { get; set; }
    public string Transmision { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public int TankCapacity { get; set; }
    public string Pic1 { get; set; }
    public string Pic2 { get; set; }
    public string Pic3 { get; set; }
    public int OwnerId { get; set; }
    
    [ForeignKey("OwnerId")]
    public User Owner { get; set; }

    public ICollection<User> UsersWhoFavorited { get; set; }
    public ICollection<CarRental> Rentals { get; set; }
}